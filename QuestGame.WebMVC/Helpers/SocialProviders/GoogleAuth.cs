using System;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class GoogleAuth : SocialProvider
    {
        public GoogleAuth()
        {
            appParams = new SocialAppParams
            {
                ClientId = "803183701728-q1ktbmuhces4vdj9udkmatn0gota8he8.apps.googleusercontent.com",
                ClientSecret = "yXNNeyD0OlL7pS-yfSzGL4bv",
                RedirectUri = "https://localhost:44366/ExternalLogin/GoogleAuthCallback",
                Scope = "email"
            };

            appPaths = new SocialAppPaths
            {
                AppGetCodePath = "https://accounts.google.com/o/oauth2/auth",
                AppGetTokenPath = "https://accounts.google.com/o/oauth2/token",
                AppGetUserInfoPath = "https://www.googleapis.com/oauth2/v1/userinfo"
            };

            getToken = new GetGoggleToken(appParams, appPaths);
            getUserInfo = new GetGoogleUserInfo(appParams, appPaths);
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
                parameters["scope"] = this.appParams.Scope;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }


    }
}