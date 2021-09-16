using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard
{
    public class TwitchIrcClient
    {

        public TwitchIrcClient()
        {
            _channels = new List<string>();
            _isConnected = false;
        }

        ~TwitchIrcClient()
        {
            Close();
        }

        public void Connect(string nick, string pass)
        {
            _irc = new TcpClient("irc.chat.twitch.tv",6667);
            NetworkStream ns = _irc.GetStream();
            _reader = new StreamReader(ns);
            _writer = new StreamWriter(ns);
            _loginData = new[] {nick, pass};
            Login(nick,pass);
            SendMsgIrc("CAP REQ :twitch.tv/tags");
            SendMsgIrc("CAP REQ :twitch.tv/membership");
            _tokenSource = new CancellationTokenSource();
            if (!_isConnected) OnConnect?.Invoke(this,EventArgs.Empty);
            _isConnected = true;
            ReadMsg(_tokenSource.Token);
        }

        public void Connect()
        {
            Connect("justinfan98121","password");
        }

        public void SendMsgIrc(string msg)
        {
            _writer.WriteLine(msg);
            _writer.Flush();
        }
        
        public void SendMsgChannel(string channel, string msg)
        {
            _writer.WriteLine($"PRIVMSG {channel} :{msg}");
            _writer.Flush();    
        }

        public void Join(string channel)
        {
            _channels.Add(channel);
            SendMsgIrc($"JOIN {channel}");
        }
        
        public void Part(string channel)
        {
            _channels.Remove(channel);
            SendMsgIrc($"PART {channel}");
        }
        
        private async void ReadMsg(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    string msg = await ReadLine(token);
                    OnRawMsg?.Invoke(this, new RawMsgEventArgs(msg));
                    string[] splitMsg = msg.Split(' ');
                    switch (splitMsg[0])
                    {
                        case "RECONNECT":
                            {
                                Close();
                                Connect(_loginData[0], _loginData[1]);
                                Join(String.Join(",", _channels));
                                break;
                            }
                        case "PING":
                            {
                                SendMsgIrc("PONG");
                                break;
                            }
                        default:
                            UserMembershipHandler(msg);
                            break;
                    }
                }
                catch (Exception _)
                {

                }
            }
        }

        private void UserMembershipHandler(string msg)
        {
            string[] splitMsg = msg.Split(' ');
            string nickname = msg.Split('!')[0].Substring(1);
            switch (splitMsg[1])
            {
                case "JOIN":
                {
                    OnJoin?.Invoke(this,new MembershipEventArgs(splitMsg[2],nickname));
                    break;
                }
                case "PART":
                {
                    OnPart?.Invoke(this,new MembershipEventArgs(splitMsg[2],nickname));
                    break;
                }
                default:
                    if (splitMsg[2] == "PRIVMSG") UserMsgHandler(msg);
                    break;
            }

        }
        
        private void UserMsgHandler(string msg)
        {
            string[] splitData = msg.Split(new []{" :"}, StringSplitOptions.None);
            List<string> userMsgList = new List<string>(splitData).GetRange(2, splitData.Length - 2);
            string userMsg = String.Join(" :", userMsgList);
            string msgType = "chat";
            MsgEventArgs msgEventArgs = new MsgEventArgs();
            msgEventArgs.User = splitData[1].Split('!')[0];
            msgEventArgs.Channel = splitData[1].Split(' ')[2];
            if (userMsg.Split(' ')[0].Contains("ACTION"))
            {
                userMsgList = new List<string>(userMsg.Split(' '));
                userMsg = String.Join("  ", userMsgList.GetRange(1, userMsgList.Count - 1));
                userMsg = userMsg.Substring(0, userMsg.Length - 1);
                msgType = "action";
            }

            msgEventArgs.MessageType = msgType;
            msgEventArgs.Message = userMsg;
            msgEventArgs.Tags = ParseTags(splitData[0].Substring(1, splitData[0].Length - 1));
            msgEventArgs.Time = DateTime.Now.Ticks;
            OnMessage?.Invoke(this,msgEventArgs);
            if (msgType=="action") OnAction?.Invoke(this,msgEventArgs);
            else OnChat?.Invoke(this,msgEventArgs);
        }

        private MsgEventArgs.TwitchTags ParseTags(string raw)
        {
            MsgEventArgs.TwitchTags tags = new MsgEventArgs.TwitchTags();
            var dTags = new Dictionary<string, string>();
            foreach (var tagsRaw in raw.Split(';'))
            {
                string[] splitTagsRaw = tagsRaw.Split('=');
                dTags.Add(splitTagsRaw[0], splitTagsRaw[1]);
            }

            if (dTags.ContainsKey("badges-info")) tags.BadgesInfo = dTags["badges-info"];
            if (dTags.ContainsKey("badges"))
            {
                if (dTags["badges"].Length > 0)
                {
                    var badges = new Dictionary<string, string>();
                    foreach (var badge in dTags["badges"].Split(','))
                    {
                        string[] badgeSplit = badge.Split('/');
                        badges.Add(badgeSplit[0],badgeSplit[1]);
                    }

                    tags.Badges = badges;
                }
            }
            if (dTags.ContainsKey("client-nonce")) tags.ClientNonce = dTags["client-nonce"];
            if (dTags.ContainsKey("bits")) tags.BadgesInfo = dTags["bits"];
            if (dTags.ContainsKey("color") && dTags["color"].Length > 0) tags.Color = dTags["color"];
            if (dTags.ContainsKey("display-name")) tags.DisplayName = dTags["display-name"];
            if (dTags.ContainsKey("emotes") && dTags["emotes"].Length > 0)
            {
                var emotes = new Dictionary<string, Pair<string,string>>();
                foreach (var emote in dTags["emotes"].Split(','))
                {
                    string[] emoteSplit = emote.Split(':');
                    string[] bordersSplit;
                    if (emoteSplit.Length > 1) bordersSplit = emoteSplit[1].Split('-');
                    else bordersSplit = new[]{"0","-1"};
                    Pair<string,string> emoteBorders = new Pair<string,string>(bordersSplit[0],bordersSplit[1]);
                    emotes.Add(emoteSplit[0],emoteBorders);
                }
                tags.Emotes = emotes;
            }
            if (dTags.ContainsKey("id")) tags.Id = dTags["id"];
            if (dTags.ContainsKey("mod")) tags.Mod = dTags["mod"]=="1";
            if (dTags.ContainsKey("reply-parent-display-name")) tags.ReplyParentDisplayName = dTags["reply-parent-display-name"];
            if (dTags.ContainsKey("reply-parent-msg-body")) tags.ReplyParentMsgBody = dTags["reply-parent-msg-body"];
            if (dTags.ContainsKey("reply-parent-msg-id")) tags.ReplyParentMsgId = dTags["reply-parent-msg-id"];
            if (dTags.ContainsKey("reply-parent-user-id")) tags.ReplyParentUserId = dTags["reply-parent-user-id"];
            if (dTags.ContainsKey("reply-parent-user-login")) tags.ReplyParentUserLogin = dTags["reply-parent-user-login"];
            if (dTags.ContainsKey("subscriber")) tags.Mod = dTags["subscriber"]=="1";
            if (dTags.ContainsKey("turbo")) tags.Mod = dTags["turbo"]=="1";
            if (dTags.ContainsKey("user-id")) tags.BadgesInfo = dTags["user-id"];
            if (dTags.ContainsKey("user-type")) tags.BadgesInfo = dTags["user-type"];
            
            return tags;
        }

        private Task<string> ReadLine(CancellationToken token)
        {
            using (token.Register(Close))
            {
                try
                {
                    return _reader.ReadLineAsync();
                }
                catch (Exception ex)
                {
                    return null; 
                }
            }
            
        }

        public void Close()
        {
            _tokenSource.Cancel();
            _irc.Close();
            _reader.Close();
            _writer.Close();
            _reader.Dispose();
            _writer.Dispose();
        }

        private void Login(string nick,string pass)
        {
            SendMsgIrc($"PASS {pass}");
            SendMsgIrc($"NICK {nick}");
        }

        private bool _isConnected;
        private CancellationTokenSource _tokenSource;
        private string[] _loginData;
        private List<string> _channels;
        private TcpClient _irc;
        private StreamReader _reader;
        private StreamWriter _writer;
        
        public event EventHandler OnMessage;
        public event EventHandler OnAction;
        public event EventHandler OnChat;
        public event EventHandler OnJoin;
        public event EventHandler OnPart;
        public event EventHandler OnConnect;
        public event EventHandler OnRawMsg;
    }

    public class MsgEventArgs : EventArgs
    {
        public string MessageType { get; set; }
        public string Channel { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        
        public long Time { get; set; }
        public TwitchTags Tags { get; set; }

        public class TwitchTags
        {
            [JsonPropertyName("badges-info")]
            public string BadgesInfo { get; set; }
            [JsonPropertyName("badges")]
            public Dictionary<string, string> Badges{ get; set; }
            [JsonPropertyName("client-nonce")]
            public string ClientNonce{ get; set; }
            [JsonPropertyName("bits")]
            public string Bits{ get; set; }
            [JsonPropertyName("color")]
            public string Color{ get; set; }
            [JsonPropertyName("display-name")]
            public string DisplayName{ get; set; }
            [JsonPropertyName("emotes")]
            public Dictionary<string,Pair<string,string>> Emotes{ get; set; }
            [JsonPropertyName("id")]
            public string Id{ get; set; }
            [JsonPropertyName("mod")]
            public bool Mod{ get; set; }
            [JsonPropertyName("reply-parent-display-name")]
            public string ReplyParentDisplayName{ get; set; }
            [JsonPropertyName("reply-parent-msg-body")]
            public string ReplyParentMsgBody{ get; set; }
            [JsonPropertyName("reply-parent-msg-id")]
            public string ReplyParentMsgId{ get; set; }
            [JsonPropertyName("reply-parent-user-id")]
            public string ReplyParentUserId{ get; set; }
            [JsonPropertyName("reply-parent-user-login")]
            public string ReplyParentUserLogin{ get; set; }
            [JsonPropertyName("subscriber")]
            public bool Subscriber{ get; set; }
            [JsonPropertyName("turbo")]
            public bool Turbo{ get; set; }
            [JsonPropertyName("user-id")]
            public string UserId{ get; set; }
            [JsonPropertyName("user-type")]
            public string UserType{ get; set; }
        }
    }
    
    public class RawMsgEventArgs : EventArgs
    {
        public RawMsgEventArgs(string msg)
        {
            RawMessage = msg;
        }
        public string RawMessage { get; set; }
    }

    public class MembershipEventArgs : EventArgs
    {
        public MembershipEventArgs(string channel,string nickname)
        {
            this.channel = channel;
            this.nickname = nickname;
        }
        public string nickname { get; set; }
        public string channel { get; set; }
    }
    
    public class Pair<T, U> {
        public Pair() {
        }

        public Pair(T first, U second) {
            this.First = first;
            this.Second = second;
        }
        [JsonPropertyName("first")]
        public T First { get; set; }
        [JsonPropertyName("second")]
        public U Second { get; set; }
    };
}