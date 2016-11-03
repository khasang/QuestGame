
using QuestGame.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviderFactory
{
    public class FaceBookAuth1 : SocialProvider1
    {
        public FaceBookAuth1()
        {
            this.RedirectUri = "https://localhost:44366/ExternalLogin/FaceBookAuthCallback";
            this.ClientId = "1601644850130436";
            this.ClientSecret = "5ca60a2235c69ed57cb4aa43685e84cc";
            this.ApplicationAuthPath = "https://www.facebook.com/v2.8/dialog/oauth";
            this.ApplicationAuthTokenPath = "https://graph.facebook.com/v2.8/oauth/access_token";
            this.RequestUserInfoPath = "https://graph.facebook.com/me";
            this.ApplicationScope = "";
        }

        public override string ProviderName()
        {
            return "FaceBook";
        }

        public override string RequestTokenUrl
        {
            get
            {
                var uriBuilder = new UriBuilder(this.ApplicationAuthTokenPath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["client_id"] = this.ClientId;
                parameters["redirect_uri"] = this.RedirectUri;
                parameters["client_secret"] = this.ClientSecret;
                parameters["code"] = this.Code;

                uriBuilder.Query = parameters.ToString();
                return uriBuilder.Uri.ToString();
            }
        }

        protected override string GetSocialToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Referer", this.RedirectUri);

                var response = client.GetAsync(this.RequestTokenUrl).Result;
                var responseData = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;

                return responseData["access_token"];
            }
        }

        public override async Task<string> GetSocialUserEmail()
        {
            var useremail = "";
            var userId="";

            using (var client = RestHelper.Create())
            {
                var request = await client.GetAsync(this.RequestUserInfo);
                var answer = await request.Content.ReadAsAsync<Dictionary<string, string>>();
                userId = answer["id"];
            }

            using (var client = RestHelper.Create())
            {
                var urrl = "https://graph.facebook.com/" + userId + "?access_token=" + this.AccessToken;
                var request = await client.GetAsync(urrl);
                var answer = await request.Content.ReadAsAsync<Dictionary<string, string>>();
                useremail = answer["email"];
            }

            return useremail;
        }
    }
}