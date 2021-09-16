using System;
using System.Threading;

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
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;
            _twitchIrc = new IrcClient("irc.chat.twitch.tv",6667);

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