using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public class Requests
    {
        private HttpClient _client;
        private string _id;
        private string _secret;

        public Requests()
        {
            _client = new HttpClient();
            _id = Environment.GetEnvironmentVariable("BOT_CLIENT_ID");
            _secret = Environment.GetEnvironmentVariable("BOT_SECRET");
        }
        
        
        public string TokenRequest()
        {
            string result = _client
                 .PostAsync(
                     "https://id.twitch.tv/oauth2/token?client_id=" + _id + "&client_secret=" + _secret +
                     "&grant_type=client_credentials", new StringContent("")).Result.Content.ReadAsStringAsync().Result;
            
             dynamic token = JObject.Parse(result)["access_token"];
             return "Bearer " +token.ToString();
        }

        private HttpRequestMessage SetRequestHelix(HttpMethod method, Uri uri)
        {
            return new HttpRequestMessage
            {
                Method = method,
                RequestUri = uri,
                Headers =
                {
                    {"Client-ID", _id},
                    {"Authorization", TokenRequest()}
                }
            };
        }

        public bool IsOnline(string name)
        {
            var request = SetRequestHelix(HttpMethod.Get,
                new Uri("https://api.twitch.tv/helix/streams?user_login=" + name));
            string response = _client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            string item = JObject.Parse(response)["data"].ToString();
            return item.Length > 2;
        }
        
        public string GetUserInfo(string name)
        {
            var request = SetRequestHelix(HttpMethod.Get,
                new Uri("https://api.twitch.tv/helix/users?login=" + name));
            string response = _client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            dynamic item = JObject.Parse(response)["data"][0];
            return item.ToString();
        }
        public string[] GetStreamerInfo(string name)
        {
            dynamic item = JObject.Parse(GetUserInfo(name));
            string[] data = new string[] {item["display_name"], item["profile_image_url"], IsOnline(name) ? "Online" : "Offline"};
            return data;
        }
    }
}