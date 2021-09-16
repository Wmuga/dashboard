using System;
using System.Net.Http;
using SocketIOClient;

namespace Dashboard
{
    public delegate void StringDataHandler(string response);
    public class NgrokClient
    {
        public NgrokClient()
        {

            _ngrokClient = new SocketIO($"http://{Environment.GetEnvironmentVariable("IP")}:3001/");
            _ngrokClient.ConnectAsync();
            _ngrokClient.OnConnected += (sender, args) => _ngrokClient.EmitAsync("eventSub");
        }
        public string GetUrl()
        {
            HttpClient client = new HttpClient();
            return client.GetAsync($"http://{Environment.GetEnvironmentVariable("IP")}:3000/").Result.Content.ReadAsStringAsync().Result;
        }

        public void EmitData(string eventName, object data)
        {
            _ngrokClient.EmitAsync(eventName,data);
        }

        public void SetCallback(string eventName, StringDataHandler callback)
        {
            _ngrokClient.On(eventName, response => { callback(response.GetValue<string>());});
        }

        public void SetCallback(StringDataHandler callback)
        {
            _ngrokClient.OnAny((name, response) => callback(name+"\r\n"+response.GetValue<string>()));
        }
        
        
        ~NgrokClient()
        {
            _ngrokClient.DisconnectAsync().Wait();
        }

        private SocketIO _ngrokClient;
    }
}