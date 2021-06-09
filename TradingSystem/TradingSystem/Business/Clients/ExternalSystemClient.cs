using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business
{
    public class ExternalSystemClient
    {
        readonly HttpClient client = new HttpClient();

        public ExternalSystemClient()
        {
            client.Timeout = TimeSpan.FromSeconds(3);
        }

        public async Task<string> Send(Dictionary<string, string> postContent)
        {
            //var url = ConfigurationManager.AppSettings["ExternalSystemsURL"];

            var url = MyConfig.Get<string>("ExternalSystemsURL");
            Console.WriteLine("url is : "+ url);

            var content = new FormUrlEncodedContent(postContent);

            var response = await client.PostAsync(url, content);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
