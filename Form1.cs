using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public partial class Form1 : Form
    {
        private void InitIrc()
        {
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;
            _twitchIrc = new IrcClient("irc.chat.twitch.tv",6667);
            _readThread = new Thread(ReadIrc);
            _updateThread = new Thread(SetLists);
            _isRunning = true;
            _readThread.Start();
            _updateThread.Start();
            _twitchIrc.Login("justinfan9812","justchillin");
            _twitchIrc.Join("#wmuga");
            _twitchIrc.SendMsgIrc("CAP REQ :twitch.tv/tags");
            _twitchIrc.SendMsgIrc("CAP REQ :twitch.tv/membership");
        }

        private void InitSqlite()
        {
            if (!File.Exists("streamDB.db")) SQLiteConnection.CreateFile("streamDB.db");
            _sqlc = new SQLiteConnection("Data Source=streamDB.db; Version=3;");
            SQLiteCommand initViewersTableCommand = new SQLiteCommand(
                "create table if not exists viewers(nickname varchar(20));",_sqlc);
            SQLiteCommand initEventsTableCommand = new SQLiteCommand(
                "create table if not exists events(id smallint,event_type varchar(50), nickname varchar(20));",_sqlc);
            _sqlc.Open();
            initViewersTableCommand.ExecuteNonQuery();
            initEventsTableCommand.ExecuteNonQuery();

            SQLiteCommand getLastId = new SQLiteCommand(
                "select coalesce(MAX(id),0) id from events", _sqlc);
            using (SQLiteDataReader reader = getLastId.ExecuteReader())
            {
                while (reader.Read())
                {
                    _lastId = reader.GetInt16(0);
                }
            }
        }

        private void InitEventSub()
        {
            _eventSub = new EventSubServer(Port);
            _eventSub.Start(_ct);
        }

 
        
        private void button1_Click(object sender, EventArgs e)
        {
            int[] subNums = {1, 7, 8, 9, 10, 11, 17, 19};
            foreach (int subNum  in subNums)
            {
                HttpResponseMessage response = _eventSub.SetSubscription((SubscriptionType) subNum, AddEvent, 164555591);
                AddText(response.Content.ReadAsStringAsync().Result+"\r\n");
            }

            button1.Enabled = false;
        }
        
        public Form1()
        {
            InitializeComponent();
            this.Load += InitInnerComponents;
            Application.ApplicationExit += ExitHandler;
            //FormClosing += ExitHandler;
        }

        private void InitInnerComponents(object sender, EventArgs e)
        {
            InitSqlite();
            InitIrc();
            InitEventSub();
        }

        private void ExitHandler(object sender, EventArgs e)
        {
            TerminateThread();
            new SQLiteCommand("delete from viewers",_sqlc).ExecuteNonQuery();
            _sqlc.Close();
        }

        private void TerminateThread()
        {
            _isRunning = false;
            _cts.Cancel();
            _readThread.Abort();
            _updateThread.Abort();
            _readThread.Join();
            _updateThread.Join();
        }

        private void ReadIrc()
        {
            while(_isRunning)
            {
                string message = _twitchIrc.ReadLine(_ct);
                MainMessageHandler(message);
                Thread.Sleep(10);
            }

        }

        private void SetLists()
        {
            while (_isRunning)
            {
                SQLiteCommand getViewersCommand = new SQLiteCommand("select * from viewers;", _sqlc);
                List<string> viewers = new List<string>();
                using(SQLiteDataReader sqldr = getViewersCommand.ExecuteReader())
                {
                    while (sqldr.Read())
                    {
                        try
                        {
                            viewers.Add(sqldr["nickname"].ToString());
                        }
                        catch (Exception ex)
                        {
                            AddText(ex.Message);
                        }
                    }
                }
                
                currentViewersBox.Text = viewers.Count>0 
                    ?  String.Join(", ", viewers)
                    : "";

                SQLiteCommand getEventsCommand = new SQLiteCommand(
                    "select * from events order by id desc limit 10", _sqlc);
                List<string> events = new List<string>();
                using (SQLiteDataReader reader = getEventsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add($"{reader["event_type"]} by {reader["nickname"]}");
                    }
                }
                events.Reverse();
                eventTextBox.Text = events.Count > 0
                    ? String.Join("\r\n", events)
                    : ""; 
                Thread.Sleep(1000);
            }
        }
        

        private void AddText(string msg)
        {
            IrcMsgBox.Text += msg == null ? "NULL\r\n" : msg;
            IrcMsgBox.SelectionStart = IrcMsgBox.Text.Length;
            IrcMsgBox.ScrollToCaret();
        }
        
        private void AddEvent(string eventData)
        {
            dynamic parsedData = JObject.Parse(eventData);
            string sType = (string) parsedData["subscription"]["type"];
            sType = CamelCaseToDotCase.ConvertBack(sType);
            SubscriptionType type = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), sType);
            string nickname;
            switch (type)
            {
                case SubscriptionType.ChannelBan:
                case SubscriptionType.ChannelUnban: 
                case SubscriptionType.ChannelModeratorAdd:    
                case SubscriptionType.ChannelModeratorRemove:   
                case SubscriptionType.ChannelFollow:
                    nickname = (string) parsedData["event"]["user_name"];
                    break;
                case SubscriptionType.ChannelRaid:
                    nickname = (string) parsedData["event"]["from_broadcaster_user_name"];
                    break;  
               case SubscriptionType.ChannelPollBegin: 
               case SubscriptionType.ChannelPollEnd:
                   nickname = (string) parsedData["event"]["broadcaster_user_name"];
                   break;
               default:
                   nickname = "NotImplemented";
                   break;
            }
            SQLiteCommand addEventCommand = new SQLiteCommand(
                "insert into events (id,event_type,nickname) values(@id,@event_type,@nickname);", _sqlc);
            addEventCommand.Parameters.AddWithValue("@id", _lastId++);
            addEventCommand.Parameters.AddWithValue("@event_type", sType);
            addEventCommand.Parameters.AddWithValue("@nickname", nickname);
            AddText($"{sType} by {nickname}");
            addEventCommand.ExecuteNonQuery();
        }

        private bool _isRunning;
        private CancellationTokenSource _cts;
        private CancellationToken _ct;
        private IrcClient _twitchIrc;
        private Thread _readThread;
        private Thread _updateThread;
        private SQLiteConnection _sqlc;
        private EventSubServer _eventSub;
        private const int Port = 3000;
        private int _lastId;
        
    }
}