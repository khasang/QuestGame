using QuestGame.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        private string code;
        public string RedirectUri { get; set; }
        protected string ClientId { get; set; }
        protected string ClientSecret { get; set; }
        protected string ApplicationAuthPath { get; set; }
        protected string ApplicationAuthTokenPath { get; set; }
        protected string RequestUserInfoPath { get; set; }
        protected string ApplicationScope { get; set; }

        public virtual string RequestAuth
        {
            get
            {
                var uriBuilder = new UriBuilder(this.ApplicationAuthPath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = this.ClientId;
                parameters["redirect_uri"] = this.RedirectUri;
                parameters["scope"] = this.ApplicationScope;
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

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;

                this.AccessToken = this.GetSocialToken();
            }
        }


        protected virtual string GetSocialToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("Referer", this.RedirectUri);

                var response =  client.PostAsync(this.RequestTokenUrl, this.RequestTokenContent).Result;
                var responseData =  response.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                return responseData["access_token"];
            }
        }

        public virtual async Task<string> GetSocialUserEmail()
        {
            var useremail = "";

            using (var client = RestHelper.Create())
            {
                var request = await client.GetAsync(this.RequestUserInfo);
                var answer = await request.Content.ReadAsAsync<Dictionary<string, string>>();
                useremail = answer["email"];
            }

            return useremail;
        }


        public abstract string ProviderName();
    }
}