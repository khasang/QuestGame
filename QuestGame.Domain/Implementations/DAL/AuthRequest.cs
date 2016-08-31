using QuestGame.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Web.SessionState;

namespace QuestGame.Domain.Implementations
{
    public class AuthRequest : IRequest
    {
        public string Token { get; set; }

        private HttpClient client;

        public AuthRequest( string token )
        {
            this.Token = token;

            this.client = new HttpClient();
            client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
        }

        public async Task<HttpResponseMessage> PostRequestAsync(string requestUri, string value)
        {
            var response = new HttpResponseMessage();

            try
            {
               // response = await client.PostAsJsonAsync(requestUri, value);
            }
            finally
            {
                client.Dispose();
            }

            return response;
        }


        public async Task<HttpResponseMessage> GetRequestAsync(string requestUri)
        {
            var response = new HttpResponseMessage();

            try
            {
                response = await client.GetAsync(requestUri);
            }
            finally
            {
                client.Dispose();
            }

            return response;
        }
    }
}
