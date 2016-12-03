using System;
using System.Web;
using System.Web.Configuration;
using QuestGame.WebMVC.Models;
using QuestGame.Common.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class GoogleAuth : SocialProvider
    {
        public GoogleAuth()
            : base(WebConfigurationManager.AppSettings["GoogleProvider"])
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
                parameters["redirect_uri"] = this.RedirectUri;
                parameters["scope"] = this.Scope;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public override SocialUserModel GetUserInfo()
        {
            var uriBuilder = new UriBuilder(this.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["access_token"] = this.AccessToken;
            uriBuilder.Query = parameters.ToString();

            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                string avatar;

                if (!String.IsNullOrEmpty(answer["picture"]))
                {
                    avatar = answer["picture"];
                }
                else
                {
                    avatar = WebConfigurationManager.AppSettings["RemoteNoImageAvailable"];
                }


                return new SocialUserModel
                {
                    SocialId = answer["id"],
                    Email = answer["email"],
                    Password = answer["id"],
                    NickName = answer["name"],
                    AvatarUrl = avatar,
                    Provider = this.Provider
                };
            }
        }

        protected override string GetToken()
        {
            var requestParams = new Dictionary<string, string>
                    {
                        { "client_id", this.ClientId },
                        { "client_secret", this.ClientSecret },
                        { "redirect_uri", this.RedirectUri },
                        { "grant_type", "authorization_code" },
                        { "code", this.Code}
                    };
            var content = new FormUrlEncodedContent(requestParams);

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://localhost:44366/");
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["WebApiServiceBaseUrl"]);

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