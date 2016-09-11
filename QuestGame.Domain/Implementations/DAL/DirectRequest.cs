using QuestGame.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Collections.Generic;

namespace QuestGame.Domain.Implementations
{
    public class DirectRequest : IRequest
    {
        private HttpClient client;
        private Dictionary<string, string> urlParams;

        /// <summary>
        /// HttpClient без авторизации 
        /// </summary>
        public DirectRequest()
        {
            urlParams = new Dictionary<string, string>();
            this.ClearParams();

            this.client = new HttpClient();
            client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            var content = new FormUrlEncodedContent(this.urlParams);

            var response = new HttpResponseMessage();

            try
            {
                response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");

            }
            finally
            {
                client.Dispose();
            }

            return response;
        }


        public async Task<HttpResponseMessage> DeleteRequestAsync(string requestUri)
        {
            var response = new HttpResponseMessage();

            try
            {
                response = await client.DeleteAsync(requestUri);
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
