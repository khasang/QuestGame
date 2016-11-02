using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviderFactory
{

    public static class SocialProviderCreator
    {
        public static SocialProvider Create(string providerName)
        {
            Type T = Type.GetType("QuestGame.WebMVC.Helpers.SocialProviderFactory." + providerName);
            object Obj = Activator.CreateInstance(T);
            return (SocialProvider)Obj;
        }
    }


    public abstract class SocialProvider
    {
        public string AccessToken { get; set; }
        public string Code { get; set; }
        protected string RedirectUri { get; set; }
        protected string ClientId { get; set; }
        protected string ClientSecret { get; set; }
        protected string ApplicationAuthPath { get; set; }
        protected string ApplicationAuthTokenPath { get; set; }
        protected string RequestUserInfoPath { get; set; }

        public virtual string RequestAuth
        {
            get
            {
                var uriBuilder = new UriBuilder(this.ApplicationAuthPath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = this.ClientId;
                parameters["redirect_uri"] = this.RedirectUri;
                parameters["scope"] = "openid profile email";
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public virtual string RequestToken
        {
            get
            {
                var uriBuilder = new UriBuilder(this.ApplicationAuthTokenPath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["client_id"] = this.ClientId;
                parameters["client_secret"] = this.ClientSecret;
                parameters["redirect_uri"] = this.RedirectUri;
                parameters["grant_type"] = "authorization_code";
                parameters["code"] = this.Code;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public virtual string RequestUserInfo
        {
            get
            {
                var uriBuilder = new UriBuilder(this.RequestUserInfoPath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["access_token"] = this.AccessToken;
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public abstract string ProviderName();
    }
}