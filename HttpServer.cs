using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public delegate void ResponseHandler(HttpListenerContext ctx);
    public delegate void StringResponseHandler(string response);

    public enum SubscriptionType
    {
        ChannelUpdate,
        ChannelFollow,
        ChannelSubscribe,
        ChannelSubscriptionEnd,
        ChannelSubscriptionGift,
        ChannelSubscriptionMessage,
        ChannelCheer,
        ChannelRaid,
        ChannelBan,
        ChannelUnban,
        ChannelModeratorAdd,
        ChannelModeratorRemove,
        ChannelCustom_point_rewardAdd,
        ChannelCustom_point_rewardUpdate,
        ChannelCustom_point_rewardRemove,
        ChannelCustom_point_reward_redemptionAdd,
        ChannelCustom_point_reward_redemptionUpdate,
        ChannelPollBegin,
        ChannelPollProgress,
        ChannelPollEnd,
        ChannelPredictionBegin,
        ChannelPredictionProgress,
        ChannelPredictionLock,
        ChannelPredictionEnd,
        ExtentionBits_transactionCreate,
        ChannelHype_trainBegin,
        ChannelHype_trainProgress,
        ChannelHype_trainEnd,
        StreamOnline,
        StreamOffline,
        UserAuthorizationRevoke,
        UserUpdate
    }

    public static class CamelCaseToDotCase
    {
        public static string Convert(string camelCase)
        {
            char[] dotCase = camelCase.ToCharArray();
            dotCase[0] = Char.ToLower(dotCase[0]);
            string res = String.Join("",dotCase);
            for (int i = dotCase.Length-1; i >= 0; i--)
            {
                if (Char.IsLetter(dotCase[i]) && Char.IsUpper(dotCase[i]))
                {
                    res = res.Replace(dotCase[i].ToString(), $".{Char.ToLower(dotCase[i])}");
                }
            }

            return res;
        }

        public static string ConvertBack(string dotCase)
        {
            char[] dotCaseAr = dotCase.ToCharArray();
            dotCaseAr[0] = char.ToUpper(dotCaseAr[0]);
            string camelCase = String.Join("", dotCaseAr);
            for (int i = dotCaseAr.Length - 2; i >= 0; i--)
            {
                if (dotCaseAr[i] == '.')
                {
                    camelCase = camelCase.Replace($".{dotCaseAr[i + 1]}", $"{Char.ToUpper(dotCaseAr[i+1])}");
                }
            }
            
            return camelCase;
        }
    }
    
    public class HttpServer
    {
        public HttpServer(string url)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://{url}");
            _listener.Start();
        }

        public async void SetResponse(ResponseHandler rh, CancellationToken ct)
        {
            using (ct.Register(_listener.Abort))
            {
                try
                {
                    rh(await _listener.GetContextAsync());
                }
                catch (Exception e)
                {
                }
            }
        }

        ~HttpServer()
        {
            _listener.Stop();
        }
        protected HttpListener _listener;
    }
    
    public class EventSubServer{

        public EventSubServer()
        {
            Requests r = new Requests();
            _appToken = r.TokenRequest();
            _client = new NgrokClient();
            CloseAll();
        }
        
        
        public void Start(StringDataHandler callback)
        {
            _client.SetCallback("data",callback);
        }
        
        
        public void SetCallback(string eventName,StringDataHandler callback)
        {
            _client.SetCallback(eventName,callback);
        }

        public void Emit(string eventName, object data)
        {
            _client.EmitData(eventName,data);
        }

        public HttpResponseMessage SetSubscription(SubscriptionType type,int id)
        {
            HttpRequestMessage msg = new HttpRequestMessage
            {
                RequestUri = new Uri("https://api.twitch.tv/helix/eventsub/subscriptions"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {"Client-ID",Environment.GetEnvironmentVariable("BOT_CLIENT_ID")},
                    {"Authorization", _appToken}
                }
            };
            string sType = CamelCaseToDotCase.Convert(type.ToString());
            string content = "{\"type\": \""+sType+"\"," +
                             "\"version\": \"1\",\"condition\": " +
                             "{\""+ (type==SubscriptionType.ChannelRaid ? "to_" : "") +"broadcaster_user_id\": \""+id+"\"}," +
                             "\"transport\": {\"method\": \"webhook\"," +
                             "\"callback\": \""+GetUrl()+"/eventsub/callback/\"," +
                             "\"secret\": \""+Environment.GetEnvironmentVariable("BOT_SECRET")+"\"}}";
            msg.Content = new StringContent(content,Encoding.UTF8,"application/json");
            HttpClient hc = new HttpClient();
            return  hc.SendAsync(msg).Result;
        }

        private string GetUrl()
        {
            return _client.GetUrl();
        }

        public HttpResponseMessage GetSubscriptions()
        {
            HttpRequestMessage msg = new HttpRequestMessage
            {
                RequestUri = new Uri("https://api.twitch.tv/helix/eventsub/subscriptions"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {"Client-ID",Environment.GetEnvironmentVariable("BOT_CLIENT_ID")},
                    {"Authorization", _appToken}
                }
            };
            HttpClient hc = new HttpClient();
            return  hc.SendAsync(msg).Result;
        }

        public void Close(string id)
        {
            HttpRequestMessage msg = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://api.twitch.tv/helix/eventsub/subscriptions?id={id}"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {"Client-ID",Environment.GetEnvironmentVariable("BOT_CLIENT_ID")},
                    {"Authorization", _appToken}
                }
            };
            HttpClient hc = new HttpClient();
            HttpResponseMessage response =  hc.SendAsync(msg).Result;
        }
        
        public void CloseAll()
        {
            dynamic subDataAr = JObject.Parse(GetSubscriptions().Content.ReadAsStringAsync().Result);
            for (int i=0;i<(int)subDataAr["total"];i++)
            {
                dynamic subData = subDataAr["data"][i];
                Close((string)subData["id"]);
            }
        }

        private NgrokClient _client;
        private string _appToken;
        
    }
}

