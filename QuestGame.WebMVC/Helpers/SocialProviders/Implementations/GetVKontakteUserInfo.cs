using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestGame.WebMVC.Models;
using QuestGame.Common.Helpers;
using Newtonsoft.Json;

namespace QuestGame.WebMVC.Helpers.SocialProviders.Implementations
{
    public class GetVKontakteUserInfo : IGetSocialUserInfo
    {
        private SocialAppParams appParams;
        private SocialAppPaths appPaths;

        public GetVKontakteUserInfo(SocialAppParams appParams, SocialAppPaths appPaths)
        {
            this.appParams = appParams;
            this.appPaths = appPaths;
        }

        public SocialUserModel GetSocialUserInfo()
        {
            var uriBuilder = new UriBuilder(this.appPaths.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["uids"] = this.appParams.SocialID;
            parameters["fields"] = this.appParams.Scope;
            parameters["access_token"] = this.appParams.AccessToken;
            uriBuilder.Query = parameters.ToString();

            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(answer);

                var resultObject = json.response[0];

                return new SocialUserModel
                {
                    SocialId = resultObject.uid,
                    Email = resultObject.screen_name,
                    NickName = resultObject.first_name + " " + resultObject.last_name,
                    AvatarUrl = resultObject.photo_big
                };
            }
        }
    }
}