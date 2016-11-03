using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using QuestGame.WebMVC.Helpers.SocialProviders;
using QuestGame.Common.Helpers;
using System.Net.Http;
using System.Collections.Generic;

namespace QuestGame.WebMVC
{
    public class GetFaceBookToken : IGetSocialToken
    {
        private SocialAppParams appParams;
        private SocialAppPaths appPaths;

        public GetFaceBookToken(SocialAppParams appParams, SocialAppPaths appPaths)
        {
            this.appParams = appParams;
            this.appPaths = appPaths;
        }

        public string GetSocialToken()
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