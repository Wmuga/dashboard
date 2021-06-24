using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Dashboard
{
    public partial class Form1
    {
        private void MainMessageHandler(string msg)
        {
            string[] splitMsg = msg.Split(' ');
            switch (splitMsg[0])
            {
                case "RECONNECT": 
                {
                    TerminateThread();
                    InitIrc();
                    break;
                }
                case "PING":
                {
                    _twitchIrc.SendMsgIrc("PONG");
                    break;
                }
                default:
                    UserMembershipHandler(msg);
                    break;
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
                    if (nickname != "justinfan9812")
                    {
                        AddText($"{nickname} joined\r\n");
                        SQLiteCommand addViewerCommand = new SQLiteCommand(
                            "insert into viewers (nickname) values(@nick);", _sqlc);
                        addViewerCommand.Parameters.AddWithValue("@nick", nickname);
                        addViewerCommand.ExecuteNonQuery();
                    }
                    break;
                }
                case "PART":
                {
                    if (nickname != "justinfan9812")
                    {
                        SQLiteCommand removeViewerCommand = new SQLiteCommand(
                            $"delete from viewers where nickname = {nickname};", _sqlc);
                        removeViewerCommand.ExecuteNonQuery();
                    }
                    break;
                }
                default:
                    if (splitMsg[2]=="PRIVMSG") UserMsgHandler(msg);
                    else AddText(msg);
                    break;
            }
        }
        private void UserMsgHandler(string msg)
        {
            string[] splitData = msg.Split(new []{" :"}, StringSplitOptions.None);
            string[] userMsgList = new List<string>(splitData).GetRange(2, splitData.Length - 2).ToArray();
            string userMsg = String.Join(" :", userMsgList);
            string user = splitData[1].Split('!')[0];
            AddText($"{user} - {userMsg}");
        }
    }
}