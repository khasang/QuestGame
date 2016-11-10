using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Configuration;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class FacebookAuth : Helpers.SocialProviders.SocialProvider
    {
        string clientId = WebConfigurationManager.AppSettings["FaceBookClientId"];

        public FacebookAuth()
        {
            


            appParams = new SocialAppParams
            {
                ClientId = "1601644850130436",
                ClientSecret = "5ca60a2235c69ed57cb4aa43685e84cc",
                RedirectUri = "https://localhost:44366/ExternalLogin/FaceBookAuthCallback",
                Scope = "id,email,name,picture.width(200).height(200)",
                Provider = "FaceBook"
            };

            appPaths = new SocialAppPaths
            {
                AppGetCodePath = "https://www.facebook.com/v2.8/dialog/oauth",
                AppGetTokenPath = "https://graph.facebook.com/v2.8/oauth/access_token",
                AppGetUserInfoPath = "https://graph.facebook.com/me/"
            };

            getToken = new GetFaceBookToken (appParams, appPaths);
            getUserInfo = new GetFaceBookUserInfo(appParams, appPaths);
        }

        public override string RequestCodeUrl
        {
            get
            {
                var uriBuilder = new UriBuilder(this.appPaths.AppGetCodePath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = this.clientId;
                parameters["redirect_uri"] = WebConfigurationManager.AppSettings["FaceBookRedirectUri"];
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        protected override string GetToken()
        {
            var uriBuilder = new UriBuilder(this.appPaths.AppGetTokenPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_id"] = this.appParams.ClientId;
            parameters["redirect_uri"] = this.appParams.RedirectUri;
            parameters["client_secret"] = this.appParams.ClientSecret;
            parameters["code"] = this.appParams.Code;
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