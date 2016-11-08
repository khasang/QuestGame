using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class FacebookAuth : Helpers.SocialProviders.SocialProvider
    {

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
                parameters["client_id"] = this.appParams.ClientId;
                parameters["redirect_uri"] = this.appParams.RedirectUri;
                //parameters["scope"] = this.appParams.Scope;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

    }
}