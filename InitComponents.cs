using System;
using System.Threading;
using System.Drawing;

namespace Dashboard
{
    public partial class Form1
    {
        private void InitInnerComponents(object sender, EventArgs e)
        {
            InitIrc();
            InitEventSub();
        }
        private void InitIrc()
        {
            _twitchIrc = new TwitchIrcClient();
            _isRunning = true;
            _twitchIrc.OnConnect += (object sender1, EventArgs e1) =>
            {
                AddСhatText("Joined\r\n", IrcMsgBox.ForeColor);
                _twitchIrc.Join("#wmuga");
                _twitchIrc.SendMsgIrc("CAP REQ :twitch.tv/tags");
                _twitchIrc.SendMsgIrc("CAP REQ :twitch.tv/membership");
            };
            SetLists();
            _twitchIrc.OnMessage += (object sender1, EventArgs e1) =>
            {
                MsgEventArgs msgEv = (MsgEventArgs)e1;
                AddСhatText(msgEv.Tags.DisplayName, ColorTranslator.FromHtml(msgEv.Tags.Color), true);
                AddСhatText(" "+ msgEv.Message + "\r\n", msgEv.MessageType == "chat" ? IrcMsgBox.ForeColor : ColorTranslator.FromHtml(msgEv.Tags.Color), false);
            };
            _twitchIrc.OnJoin += (object sender1, EventArgs e1) => _viewers.Add(((MembershipEventArgs)e1).nickname);
            _twitchIrc.OnPart += (object sender1, EventArgs e1) => _viewers.Remove(((MembershipEventArgs)e1).nickname);

            _twitchIrc.Connect("justinfan9812", "justchillin");
        }

        private void InitEventSub()
        {
            _eventSub = new EventSubServer();
            _eventSub.Start(AddEvent);
            _eventSub.SetCallback("last10", SetEvents);
        }

        private void SetEvents(string events)
        {
            eventTextBox.Text = events;
        }
    }
}