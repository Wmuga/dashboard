using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dashboard
{
    public partial class Form1 : Form
    {
        private void button1_Click(object sender, EventArgs e)
        {
            _eventSub.Emit("last10",null);
            int[] subNums = {1, 7, 8, 9, 10, 11, 17, 19};
            foreach (int subNum  in subNums)
            {
                HttpResponseMessage response = _eventSub.SetSubscription((SubscriptionType) subNum, 164555591);
            }
            buttonSetSubs.Enabled = false;
        }

        private void AddEvent(string ev)
        {
            eventTextBox.Text += ev;
        }
        
        public Form1()
        {
            InitializeComponent();
            this.Load += InitInnerComponents;
            Application.ApplicationExit += ExitHandler;
            topPanel.MouseDown += topPanel_MouseDown;
            topPanel.MouseUp += topPanel_MouseUp;
            topPanel.MouseMove += topPanel_MouseMove;
            _viewers = new List<string>();
        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _lastLocation = e.Location;
        }
        
        private void topPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                this.Location = new Point((this.Location.X - _lastLocation.X) + e.X,
                    (this.Location.Y - _lastLocation.Y) + e.Y);
            }
        }
        

        private void ExitHandler(object sender, EventArgs e)
        {
            TerminateThread();
        }

        private void TerminateThread()
        {
            _isRunning = false;
        }

        private async void SetLists()
        {
            while (_isRunning)
            {
                currentViewersBox.Text = _viewers.Count>0 
                    ?  String.Join(", ", _viewers)
                    : "";
                await Task.Delay(1000);
            }
        }
        

        private void AddСhatText(string text, Color color, bool bold = false)
        {
            IrcMsgBox.SelectionStart = IrcMsgBox.TextLength;
            IrcMsgBox.SelectionLength = 0;
            IrcMsgBox.SelectionColor = color;
            IrcMsgBox.SelectionFont = new Font(IrcMsgBox.SelectionFont.FontFamily, IrcMsgBox.SelectionFont.Size, bold ? FontStyle.Bold : FontStyle.Regular);
            IrcMsgBox.AppendText(text);
            IrcMsgBox.SelectionColor = IrcMsgBox.ForeColor;
        }

        private bool _isRunning;
        private TwitchIrcClient _twitchIrc;
        private EventSubServer _eventSub;
        private bool _mouseDown;
        private Point _lastLocation;
        private List<string> _viewers;

        private void sendMsgButton_Click(object sender, EventArgs e)
        {
            if (channelTextBox.Text.Length>0 && msgTextBox.Text.Length>0)
            {
                SendMsgToChat(msgTextBox.Text);
                msgTextBox.Text = "";
            }
        }
        
        private void startReqButton_Click(object sender, EventArgs e)
        {
            if (channelTextBox.Text.Length>0)
            {
                SendMsgToChat("!sr-start");
            }
        }
        
        private void endReqButton_Click(object sender, EventArgs e)
        {
            if (channelTextBox.Text.Length>0)
            {
                SendMsgToChat("!sr-end");
            }
        }
        
        private void skipReqButton_Click(object sender, EventArgs e)
        {
            if (channelTextBox.Text.Length>0)
            {
                SendMsgToChat("!sr-skip");
            }
        }

        private void SendMsgToChat(string message)
        {
            if (channelTextBox.Text.Length>0)
            {
                TwitchIrcClient wmugaIrc = new TwitchIrcClient();
                wmugaIrc.Connect("wmuga", Environment.GetEnvironmentVariable("IRC_OAUTH"));
                wmugaIrc.OnConnect += (object sender, EventArgs e) =>
                {
                    wmugaIrc.Join($"#{channelTextBox.Text.ToLower()}");
                    wmugaIrc.SendMsgChannel($"#{channelTextBox.Text.ToLower()}", message);
                    wmugaIrc.Close();
                    msgTextBox.Text = "";
                };
            }
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}