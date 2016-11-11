using QuestGame.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public abstract class SocialProvider
    {
        private string code;

        protected string ClientId { get; set; }
        protected string ClientSecret { get; set; }
        protected string RedirectUri { get; set; }
        protected string AccessToken { get; set; }
        protected string Scope { get; set; }
        protected string SocialID { get; set; }
        protected string Provider { get; set; }

        protected string AppGetCodePath { get; set; }
        protected string AppGetTokenPath { get; set; }
        protected string AppGetUserInfoPath { get; set; }

        public SocialProvider(string providerName)
        {
            this.Provider = providerName;

            this.ClientId = WebConfigurationManager.AppSettings[this.Provider + "ClientId"];
            this.ClientSecret = WebConfigurationManager.AppSettings[this.Provider + "ClientSecret"];
            this.RedirectUri = WebConfigurationManager.AppSettings[this.Provider + "RedirectUri"];
            this.Scope = WebConfigurationManager.AppSettings[this.Provider + "Scope"];

            this.AppGetCodePath = WebConfigurationManager.AppSettings[this.Provider + "AppGetCodePath"];
            this.AppGetTokenPath = WebConfigurationManager.AppSettings[this.Provider + "AppGetTokenPath"];
            this.AppGetUserInfoPath = WebConfigurationManager.AppSettings[this.Provider + "AppGetUserInfoPath"];
        }

        public virtual string Code
        {
            get { return this.code; }
            set
            {
                this.code = value;
                this.AccessToken = GetToken();
            }
        }

        protected abstract string GetToken();

        public abstract string RequestCodeUrl { get; }

        public abstract SocialUserModel GetUserInfo();
    }
}