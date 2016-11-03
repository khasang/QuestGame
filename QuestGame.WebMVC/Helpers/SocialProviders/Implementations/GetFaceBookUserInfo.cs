using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using QuestGame.WebMVC.Helpers.SocialProviders;
using System.Collections.Generic;
using System.Net.Http;
using QuestGame.Common.Helpers;

namespace QuestGame.WebMVC
{
    public class GetFaceBookUserInfo : IGetSocialUserInfo
    {
        private SocialAppParams appParams;
        private SocialAppPaths appPaths;

        public GetFaceBookUserInfo(SocialAppParams appParams, SocialAppPaths appPaths)
        {
            this.appParams = appParams;
            this.appPaths = appPaths;
        }

        public Dictionary<string, string> GetSocialUserInfo()
        {
            var uriBuilder = new UriBuilder(this.appPaths.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["access_token"] = this.appParams.AccessToken;
            uriBuilder.Query = parameters.ToString();
            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                return answer;
            }
        }
    }
}