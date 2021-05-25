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
        public async Task<string> Send(Dictionary<string, string> postContent)
        {
            //var url = ConfigurationManager.AppSettings["ExternalSystemsURL"];

            var url = "https://cs-bgu-wsep.herokuapp.com/";

            var content = new FormUrlEncodedContent(postContent);

            var response = await client.PostAsync(url, content);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
