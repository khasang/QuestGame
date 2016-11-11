using QuestGame.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public abstract class SocialProvider
    {
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

        public virtual string Code
        {
            get { return this.Code; }
            set
            {
                this.Code = value;
                this.AccessToken = GetToken();
            }
        }

        protected abstract string GetToken();

        public abstract string RequestCodeUrl { get; }

        public abstract SocialUserModel GetUserInfo();
    }
}