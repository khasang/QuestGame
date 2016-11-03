using System;
using System.Collections.Generic;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public abstract class SocialProvider
    {
        protected SocialAppPaths appPaths;
        protected SocialAppParams appParams;
        protected IGetSocialToken getToken;
        protected IGetSocialUserInfo getUserInfo;

        public virtual string RequestCodeUrl { get; }

        public string Code
        {
            set
            {
                appParams.Code = value;
                this.appParams.AccessToken = GetToken();
            }
        }

        protected string GetToken()
        {
            return getToken.GetSocialToken();
        }

        public Dictionary<string, string> GetUserInfo()
        {
            return getUserInfo.GetSocialUserInfo();
        }
    }
}