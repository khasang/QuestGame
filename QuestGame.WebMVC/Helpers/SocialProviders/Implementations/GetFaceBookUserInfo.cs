using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using QuestGame.WebMVC.Helpers.SocialProviders;
using System.Collections.Generic;
using System.Net.Http;
using QuestGame.Common.Helpers;
using QuestGame.WebMVC.Models;
using Newtonsoft.Json;

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

        SocialUserModel IGetSocialUserInfo.GetSocialUserInfo()
        {
            var uriBuilder = new UriBuilder(this.appPaths.AppGetUserInfoPath);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["access_token"] = this.appParams.AccessToken;
            parameters["fields"] = this.appParams.Scope;
            uriBuilder.Query = parameters.ToString();
            var queryUrl = uriBuilder.Uri.ToString();

            using (var client = RestHelper.Create())
            {
                var request = client.GetAsync(queryUrl).Result;
                var answer = request.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(answer);

                string avatar;

                bool pictureIsAviable = (bool)json.picture.data.is_silhouette;

                if (!pictureIsAviable)
                {
                    avatar = json.picture[0].data[0].url;
                }
                else
                {
                    avatar = @"http://vignette3.wikia.nocookie.net/shokugekinosoma/images/6/60/No_Image_Available.png/revision/latest?cb=20150708082716";
                }

                return new SocialUserModel
                {
                    SocialId = json.id,
                    Email = json.email,
                    Password = json.id,
                    NickName = json.name,
                    AvatarUrl = avatar,
                    Provider = appParams.Provider
                };
            }
        }
    }
}