using System;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace Dashboard
{
    public partial class Form1
    {
        private void InitInnerComponents(object sender, EventArgs e)
        {
            InitSqlite();
            InitIrc();
            InitEventSub();
        }
        private void InitIrc()
        {
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;
            _twitchIrc = new IrcClient("irc.chat.twitch.tv",6667);

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
            _eventSub = new EventSubServer();
            _eventSub.Start();
        }
    }
}