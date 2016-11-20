using System;
using System.Web;
using System.Web.Configuration;
using QuestGame.WebMVC.Models;
using QuestGame.Common.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class YandexAuth : SocialProvider
    {
        public YandexAuth() : 
            base(WebConfigurationManager.AppSettings["YandexProvider"])
        {
        }

        public override string RequestCodeUrl
        {
            get
            {
                var uriBuilder = new UriBuilder(this.AppGetCodePath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = this.ClientId;
                parameters["display"] = "'popup'";
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public override SocialUserModel GetUserInfo()
        {
            var uriBuilder = new UriBuilder(this.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["format"] = "json";
            parameters["oauth_token"] = this.AccessToken;
            uriBuilder.Query = parameters.ToString();

            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(answer);

                string avatar;

                if (!(bool)json.is_avatar_empty)
                {
                    avatar = @"http://avatars.mds.yandex.net/get-yapic/" + json.default_avatar_id + @"/islands-200" + @"?.png";
                }
                else
                {
                    avatar = @"http://vignette3.wikia.nocookie.net/shokugekinosoma/images/6/60/No_Image_Available.png";
                }

                var user = new SocialUserModel
                {
                    SocialId = json.id,
                    Email = json.emails[0],
                    Password = json.id,
                    NickName = json.real_name,
                    AvatarUrl = avatar,
                    Provider = this.Provider
                };

                return user;
            }
        }

        protected override string GetToken()
        {
            var requestParams = new Dictionary<string, string>
                    {
                        { "client_id", this.ClientId },
                        { "client_secret", this.ClientSecret },
                        { "grant_type", "authorization_code" },
                        { "code", this.Code}
                    };

            var content = new FormUrlEncodedContent(requestParams);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("Referer", this.RedirectUri);

                var response = client.PostAsync(this.AppGetTokenPath, content).Result;
                var responseData = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                return responseData["access_token"];
            }
        }
    }
}