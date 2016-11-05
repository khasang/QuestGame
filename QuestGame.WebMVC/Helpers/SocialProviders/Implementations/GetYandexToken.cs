using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using QuestGame.WebMVC.Helpers.SocialProviders;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace QuestGame.WebMVC
{
    public class GetYandexToken : IGetSocialToken
    {
        private SocialAppParams appParams;
        private SocialAppPaths appPaths;

        public GetYandexToken(SocialAppParams appParams, SocialAppPaths appPaths)
        {
            this.appParams = appParams;
            this.appPaths = appPaths;
        }

        public string GetSocialToken()
        {
            var requestParams = new Dictionary<string, string>
                    {
                        { "client_id", this.appParams.ClientId },
                        { "client_secret", this.appParams.ClientSecret },
                        //{ "redirect_uri", this.appParams.RedirectUri },
                        { "grant_type", "authorization_code" },
                        { "code", this.appParams.Code}
                    };

            var content = new FormUrlEncodedContent(requestParams);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("Referer", this.appParams.RedirectUri);

                var response = client.PostAsync(this.appPaths.AppGetTokenPath, content).Result;
                var responseData = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                return responseData["access_token"];
            }
        }
    }
}