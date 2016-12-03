using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Configuration;
using QuestGame.WebMVC.Models;
using QuestGame.Common.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class FacebookAuth : SocialProvider
    {

        public FacebookAuth()
            : base(WebConfigurationManager.AppSettings["FaceBookProvider"])
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
                //parameters["scope"] = this.appParams.Scope;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public override SocialUserModel GetUserInfo()
        {
            var uriBuilder = new UriBuilder(this.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["access_token"] = this.AccessToken;
            parameters["fields"] = this.Scope;
            uriBuilder.Query = parameters.ToString();
            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(answer);

                string avatar;

                bool pictureIsAviable = (bool)json.picture.data.is_silhouette;
                bool pictureIsEmpty = String.IsNullOrEmpty((string)@json.picture.data.url);

                if (!pictureIsAviable & !pictureIsEmpty)
                {
                    var urlString = new Uri((string)@json.picture.data.url);
                    var extensn = Path.GetExtension(urlString.LocalPath);
                    avatar = urlString.OriginalString + @"&" + extensn;
                }
                else
                {
                    avatar = WebConfigurationManager.AppSettings["RemoteNoImageAvailable"];
                }

                return new SocialUserModel
                {
                    SocialId = json.id,
                    Email = json.email,
                    Password = json.id,
                    NickName = json.name,
                    AvatarUrl = avatar,
                    Provider = this.Provider
                };
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
                var responseData = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                return responseData["access_token"];
            }
        }
    }
}