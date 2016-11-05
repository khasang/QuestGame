using Newtonsoft.Json;
using QuestGame.WebMVC.Helpers.SocialProviders.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class VKontakteAuth : SocialProvider
    {
        public VKontakteAuth()
        {
            appParams = new SocialAppParams
            {
                ClientId = "5712694",
                ClientSecret = "yFCa0WNATugPbAMqsxso",
                RedirectUri = "https://localhost:44366/ExternalLogin/VKontakteAuthCallback",
                Scope = "uid,first_name,last_name,screen_name,photo_big,email"
            };

            appPaths = new SocialAppPaths
            {
                AppGetCodePath = "http://oauth.vk.com/authorize",
                AppGetTokenPath = "https://oauth.vk.com/access_token",
                AppGetUserInfoPath = "https://api.vk.com/method/users.get"
            };

            getToken = new GetVKontakteToken(appParams, appPaths);
            getUserInfo = new GetVKontakteUserInfo(appParams, appPaths);
        }

        public override string RequestCodeUrl
        {
            get
            {
                var uriBuilder = new UriBuilder(this.appPaths.AppGetCodePath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = this.appParams.ClientId;
                parameters["redirect_uri"] = this.appParams.RedirectUri;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public override string Code
        {
            set
            {
                appParams.Code = value;
                var query = GetToken();

                dynamic json = JsonConvert.DeserializeObject(query);

                this.appParams.AccessToken = json.access_token;
                this.appParams.SocialID = json.user_id;
            }
        }
    }
}