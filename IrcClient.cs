using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard
{
    public class IrcClient
    {
        public IrcClient(string ircUri,int port)
        {
            _irc = new TcpClient(ircUri,port);
            NetworkStream ns = _irc.GetStream();
            _reader = new StreamReader(ns);
            _writer = new StreamWriter(ns);
        }

        ~IrcClient()
        {
            Close();
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
            SendMsgIrc($"JOIN {channel}");
        }

        public Task<string> ReadLine(CancellationToken token)
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
            _irc.Close();
            _reader.Close();
            _writer.Close();
            _reader.Dispose();
            _writer.Dispose();
        }

        public void Login(string nick,string pass)
        {
            SendMsgIrc($"PASS {pass}");
            SendMsgIrc($"NICK {nick}");
        }
        
        
        private TcpClient _irc;
        private StreamReader _reader;
        private StreamWriter _writer;
    }
}