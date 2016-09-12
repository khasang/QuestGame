using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Web;

namespace QuestGame.Common
{
    public class RequestApi : IDisposable
    {
        private HttpClient client;
        private string token;
        private Dictionary<string, string> sendParams;

        /// <summary>
        /// Отправка прямых, неавторизированных запросов к WebApi
        /// </summary>
        public RequestApi()
        {
            sendParams = new Dictionary<string, string>();
            this.client = new HttpClient();
            client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Отправка авторизированных запросов к WebApi. Входящий параметр - Token
        /// </summary>
        public RequestApi(string token) : this()
        {
            this.token = token;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
        }


        #region Прямые запросы  - Не проверен...

        /// <summary>
        /// Запрос Get (не асинхронный)
        /// </summary>
        /// <param name="url запроса"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string requestUri)
        {
            return client.GetAsync(requestUri).Result;
        }

        #endregion

        #region Асинхронные запросы

        /// <summary>
        /// Запрос Get (асинхронный)
        /// </summary>
        /// <param name="url запроса"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await client.GetAsync(requestUri);
        }

        /// <summary>
        /// Запрос Get (асинхронный) \ Параметризированный
        /// </summary>
        /// <param name="url запроса"></param>
        public async Task<T> GetAsync<T>(string requestUri)
        {
            var request = await client.GetAsync(requestUri);
            var response = await request.Content.ReadAsAsync<T>();
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri)
        {
            var content = new FormUrlEncodedContent(this.sendParams);
            return await client.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await client.DeleteAsync(requestUri);
        }

        #endregion

        #region Асинхронные запросы - Json

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string requestUri, T value)
        {
            try
            {
                var result = await client.PostAsJsonAsync(requestUri, value);
                result.EnsureSuccessStatusCode();
                return result;
            }
            catch (HttpException ex)
            {
                Console.WriteLine(ex);

                return new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

        }

        #endregion


        public void AddSendParam(string Key, string value)
        {
            sendParams.Add(Key, value);
        }

        public void ClearParams()
        {
            sendParams.Clear();
        }


        #region Disposse
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    client.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}