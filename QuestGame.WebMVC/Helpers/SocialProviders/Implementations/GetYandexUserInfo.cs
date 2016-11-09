using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using QuestGame.WebMVC.Helpers.SocialProviders;
using System.Collections.Generic;
using QuestGame.Common.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using QuestGame.WebMVC.Models;

namespace QuestGame.WebMVC
{
    public class GetYandexUserInfo : IGetSocialUserInfo
    {
        private SocialAppParams appParams;
        private SocialAppPaths appPaths;

        public GetYandexUserInfo(SocialAppParams appParams, SocialAppPaths appPaths)
        {
            this.appParams = appParams;
            this.appPaths = appPaths;
        }

        public SocialUserModel GetSocialUserInfo()
        {
            var uriBuilder = new UriBuilder(this.appPaths.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["format"] = "json";
            parameters["oauth_token"] = this.appParams.AccessToken;
            uriBuilder.Query = parameters.ToString();

            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(answer);

                string avatar;

                if (!(bool)json.is_avatar_empty)
                {
                    avatar = @"http://avatars.mds.yandex.net/get-yapic/" + json.default_avatar_id + @"/islands-200";
                }
                else
                {
                    avatar = @"http://vignette3.wikia.nocookie.net/shokugekinosoma/images/6/60/No_Image_Available.png/revision/latest?cb=20150708082716";
                }

                var user = new SocialUserModel {
                    SocialId = json.id,
                    Email = json.emails[0],
                    Password = json.id,
                    NickName = json.real_name,
                    AvatarUrl = avatar,
                    Provider = appParams.Provider
                };

                return user;
            }
        }
    }

}