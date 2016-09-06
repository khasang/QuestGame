using QuestGame.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Collections.Generic;

namespace QuestGame.Domain.Implementations
{
    public class AuthRequest : IRequest
    {
        public string Token { get; set; }

        private HttpClient client;
        private Dictionary<string, string> urlParams;

        /// <summary>
        /// HttpClient c авторизацией через токен
        /// </summary>
        public AuthRequest( string token )
        {
            urlParams = new Dictionary<string, string>();
            this.Token = token;
            this.ClearParams();

            this.client = new HttpClient();
            client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
        }

        public async Task<HttpResponseMessage> PostRequestAsync<T>(string requestUri, T value)
        {
            var response = new HttpResponseMessage();

            try
            {
                response = await client.PostAsJsonAsync(requestUri, value);
            }
            finally
            {
                client.Dispose();
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri)
        {
            var content = new FormUrlEncodedContent( this.urlParams );

            var response = new HttpResponseMessage();

            try
            {
                response = await client.PostAsync(requestUri, content);
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

        public void AddUrlParam(string Key, string value)
        {
            urlParams.Add(Key, value);
        }
        public void ClearParams()
        {
            urlParams.Clear();
        }
    }
}
