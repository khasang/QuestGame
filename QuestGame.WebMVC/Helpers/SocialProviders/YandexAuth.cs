using System;
using System.Web;
using System.Web.Configuration;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class YandexAuth : SocialProvider
    {
        public YandexAuth()
        {
            appParams = new SocialAppParams
            {
                ClientId = "2c5f922adc9248c5ac8ac6a6cc74b5d8",
                ClientSecret = "0885fc2c643147049560890503abf227",
                RedirectUri = "https://localhost:44366/ExternalLogin/YandexAuthCallback",
                Scope = "email",
                Provider = "Yandex"
            };

            appPaths = new SocialAppPaths
            {
                AppGetCodePath = "https://oauth.yandex.ru/authorize",
                AppGetTokenPath = "https://oauth.yandex.ru/token",
                AppGetUserInfoPath = "https://login.yandex.ru/info"
            };

            getToken = new GetYandexToken(appParams, appPaths);
            getUserInfo = new GetYandexUserInfo(appParams, appPaths);
        }

        public override string RequestCodeUrl
        {
            get
            {
                var uriBuilder = new UriBuilder(this.appPaths.AppGetCodePath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = WebConfigurationManager.AppSettings["YandexClientId"];
                parameters["display"] = "'popup'";
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }


    }
}