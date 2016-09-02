﻿using QuestGame.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;

namespace QuestGame.Domain.Implementations
{
    public class DirectRequest : IRequest
    {
        private HttpClient client;

        /// <summary>
        /// HttpClient без авторизации 
        /// </summary>
        public DirectRequest()
        {
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
    }
}