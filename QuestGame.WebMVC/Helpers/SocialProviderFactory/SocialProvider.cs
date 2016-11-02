using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public string RedirectUri { get; set; }
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
                parameters["scope"] = "email";
                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        public virtual string RequestTokenUrl { get { return this.ApplicationAuthTokenPath;  } }

        public virtual FormUrlEncodedContent RequestTokenContent
        {
            get
            {
                var requestParams = new Dictionary<string, string>
                    {
                        { "client_id", this.ClientId },
                        { "client_secret", this.ClientSecret },
                        { "redirect_uri", this.RedirectUri },
                        { "grant_type","authorization_code" },
                        { "code", this.Code}
                    };
                var content = new FormUrlEncodedContent(requestParams);

                return content;
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