using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;

namespace Dashboard
{
    public class NgrokClient
    {
        public NgrokClient(int port)
        {
            _ngrokClient = new Process();
            ProcessStartInfo psi = new ProcessStartInfo(@"D:\Programs\Ngrok\ngrok.exe");
            psi.UseShellExecute = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.Arguments = $"http {port} -host-header=\"localhost:{port}\"";
            _ngrokClient.StartInfo = psi;
            Process.Start(@"D:\Programs\Ngrok\ngrok.exe",
                $"authtoken {Environment.GetEnvironmentVariable("NGROK_AUTHTOKEN")}");
            _ngrokClient.Start();
            Thread.Sleep(100);
        }
        public string GetUrl()
        {
            HttpClient client = new HttpClient();
            dynamic response = JObject.Parse(client.GetAsync("http://localhost:4040/api/tunnels").Result.Content.ReadAsStringAsync().Result);
            string res = response["tunnels"][0]["public_url"];
            foreach (dynamic url in response["tunnels"])
            {
                if (url["proto"] == "https") res = url["public_url"];
            }
            return res;
        }
        
        ~NgrokClient()
        {
            _ngrokClient.Kill();
        }

        private Process _ngrokClient;
    }
}