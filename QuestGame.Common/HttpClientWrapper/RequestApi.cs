using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Data;
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
            try
            {
                var result = client.GetAsync(requestUri).Result;
                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(ex.Message)
                };
            }
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
            try
            {
                var result = await client.GetAsync(requestUri);
                result.EnsureSuccessStatusCode();
                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(ex.Message)
                };
            }
        }

        /// <summary>
        /// Запрос Get (асинхронный) \ Параметризированный
        /// </summary>
        /// <param name="url запроса"></param>
        public async Task<T> GetAsync<T>(string requestUri)
        {
            try
            {
                var request = await client.GetAsync(requestUri);
                request.EnsureSuccessStatusCode();
                var response = await request.Content.ReadAsAsync<T>();
                return response;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async Task<RequestApiResult> GetAsyncResult<T>(string requestUri)
        {
            var resultOk = new RequestApiResult()
            {
                Succes = true,
                Status = System.Net.HttpStatusCode.OK,
                ResponseErrors = null
            };
            var resultBad = new RequestApiResult()
            {
                Succes = false,
                Status = System.Net.HttpStatusCode.BadRequest,
                ResponseData = default(T)
            };

            try
            {
                var request = await client.GetAsync(requestUri);
                request.EnsureSuccessStatusCode();
                var response = await request.Content.ReadAsAsync<T>();

                if (response == null)
                {
                    throw new ArgumentException("Ошибка получения объекта. Воможно он удален, либо отсутствует соединение.");
                }

                resultOk.ResponseData = response;

                return resultOk;
            }
            catch (HttpRequestException ex)
            {
                resultBad.ResponseErrors.Add(ex.Message);
                return resultBad;
            }
            catch (ArgumentException ex)
            {
                resultBad.ResponseErrors.Add(ex.Message);

                return resultBad;
            }

        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri)
        {
            try
            {
                var content = new FormUrlEncodedContent(this.sendParams);
                var result = await client.PostAsync(requestUri, content);
                result.EnsureSuccessStatusCode();
                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(ex.Message)
                };
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            try
            {
                var result = await client.DeleteAsync(requestUri);
                result.EnsureSuccessStatusCode();
                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(ex.Message)
                };
            }
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
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(ex.Message)
                };
            }
        }

        /// <summary>
        /// Обновление модели - rest-Update-HttpPut
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutJsonAsync<T>(string requestUri, T value)
        {
            try
            {
                var result = await client.PutAsJsonAsync(requestUri, value);
                result.EnsureSuccessStatusCode();
                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(ex.Message)
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