using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace QuestGame.Common.Helpers
{
    public class RestHelper
    {
        public static string BaseUrl = WebConfigurationManager.AppSettings["WebApiServiceBaseUrl"];

        public static HttpClient Create()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static HttpClient Create(string authToken)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);

            return client;
        }

        public static void UploadFile(string methodUrl, string pathFile)
        {
            var client = new WebClient();
            try
            {
                client.UploadFile(BaseUrl + methodUrl, pathFile);  // Отправляем файл в web api слой
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ошибка загрузки файла: {0}", ex.ToString()));
            }
        }
    }
}
