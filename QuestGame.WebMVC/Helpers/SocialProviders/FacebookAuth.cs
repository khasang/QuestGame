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

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class FacebookAuth : Helpers.SocialProviders.SocialProvider
    {

        public FacebookAuth()
        {
            this.Provider = WebConfigurationManager.AppSettings["FaceBookProvider"];

            ClientId = WebConfigurationManager.AppSettings[this.Provider + "ClientId"];
            ClientSecret = WebConfigurationManager.AppSettings[this.Provider + "ClientSecret"];
            RedirectUri = WebConfigurationManager.AppSettings[this.Provider + "RedirectUri"];
            Scope = WebConfigurationManager.AppSettings[this.Provider + "Scope"];

            AppGetCodePath = WebConfigurationManager.AppSettings[this.Provider + "AppGetCodePath"];
            AppGetTokenPath = WebConfigurationManager.AppSettings[this.Provider + "AppGetTokenPath"];
            AppGetUserInfoPath = WebConfigurationManager.AppSettings[this.Provider + "AppGetUserInfoPath"];
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

                if (!pictureIsAviable)
                {
                    avatar = json.picture[0].data[0].url;
                }
                else
                {
                    avatar = @"http://vignette3.wikia.nocookie.net/shokugekinosoma/images/6/60/No_Image_Available.png/revision/latest?cb=20150708082716";
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