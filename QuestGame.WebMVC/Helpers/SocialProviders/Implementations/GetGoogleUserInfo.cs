using System;
using System.Web;
using System.Collections.Generic;
using QuestGame.WebMVC.Helpers.SocialProviders;
using QuestGame.Common.Helpers;
using System.Net.Http;
using QuestGame.WebMVC.Models;

namespace QuestGame.WebMVC
{
    public class GetGoogleUserInfo : IGetSocialUserInfo
    {
        private SocialAppParams appParams;
        private SocialAppPaths appPaths;

        public GetGoogleUserInfo(SocialAppParams appParams, SocialAppPaths appPaths)
        {
            this.appParams = appParams;
            this.appPaths = appPaths;
        }

        public SocialUserModel GetSocialUserInfo()
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

                return new SocialUserModel
                {
                    SocialId = answer["id"],
                    Email = answer["email"],
                    NickName = answer["name"],
                    AvatarUrl = answer["picture"]
                };
            }
        }
    }
}