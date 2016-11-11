using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Configuration;
using QuestGame.WebMVC.Models;
using QuestGame.Common.Helpers;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class VKontakteAuth : SocialProvider
    {
        private string code;

        public VKontakteAuth()
            : base (WebConfigurationManager.AppSettings["VKontakteProvider"] )
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
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public override string Code
        {
            get { return this.code; }
            set
            {
                this.code = value;
                var query = GetToken();

                dynamic json = JsonConvert.DeserializeObject(query);

                this.AccessToken = json.access_token;
                this.SocialID = json.user_id;
            }
        }



        protected override string GetToken()
        {
            var uriBuilder = new UriBuilder(this.AppGetTokenPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_id"] = this.ClientId;
            parameters["redirect_uri"] = this.RedirectUri;
            parameters["client_secret"] = this.ClientSecret;
            parameters["code"] = this.Code;
            uriBuilder.Query = parameters.ToString();

            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var response = client.GetAsync(queryUrl).Result;
                var responseData = response.Content.ReadAsStringAsync().Result;

                return responseData;
            }
        }

        public override SocialUserModel GetUserInfo()
        {
            var uriBuilder = new UriBuilder(this.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["uids"] = this.SocialID;
            parameters["fields"] = this.Scope;
            parameters["access_token"] = this.AccessToken;
            uriBuilder.Query = parameters.ToString();

            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(answer);

                var resultObject = json.response[0];

                return new SocialUserModel
                {
                    SocialId = resultObject.uid,
                    Email = resultObject.screen_name + "@VKfakeemail.ru",
                    Password = resultObject.uid,
                    NickName = resultObject.first_name + " " + resultObject.last_name,
                    AvatarUrl = resultObject.photo_big,
                    Provider = this.Provider
                };
            }
        }
    }
}