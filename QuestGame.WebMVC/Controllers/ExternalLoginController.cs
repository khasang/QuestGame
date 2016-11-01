using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Controllers
{
    public class ExternalLoginController : Controller
    {
        // GET: ExternalLogin
        public ActionResult Index()
        {
            Type T = Type.GetType("QuestGame.WebMVC.Controllers.GoogleAuth");

            object Obj = Activator.CreateInstance(T);

            SocialProvider Prov = (SocialProvider)Obj;

            var t = Prov.RequestAuth;

            var googleUrl = "https://accounts.google.com/o/oauth2/auth" +
                "?response_type=code" +
                "&client_id=803183701728-q1ktbmuhces4vdj9udkmatn0gota8he8.apps.googleusercontent.com" +
                "&redirect_uri=https://localhost:44366/ExternalLogin/GoogleCode" +
                "&scope=openid%20profile%20email";

            return Redirect(googleUrl);
        }

        [HttpGet]
        public async Task<ActionResult> GoogleCode(string code)

        {
            var google_url = "https://accounts.google.com/o/oauth2/token";


            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback = delegate { return true; };

                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri("https://localhost:44366/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var requestParams = new Dictionary<string, string>
                {
                    { "client_id", "803183701728-q1ktbmuhces4vdj9udkmatn0gota8he8.apps.googleusercontent.com" },
                    { "client_secret", "yXNNeyD0OlL7pS-yfSzGL4bv" },
                    { "redirect_uri", "https://localhost:44366/ExternalLogin/GoogleCode" },
                    { "grant_type","authorization_code" },
                    { "code", code}
                };
                    var content = new FormUrlEncodedContent(requestParams);
                    var response = await client.PostAsync(google_url, content);

                    var responseData = await response.Content.ReadAsAsync<Dictionary<string, string>>();

                    return RedirectToAction("GoogleUser", new { token = responseData["access_token"] });

                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GoogleUser(string token)
        {
            var useremail = "";

            using (var client = RestHelper.Create())
            {
                var request = await client.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + token);
                var answer = await request.Content.ReadAsAsync<Dictionary<string, string>>();
                useremail = answer["email"];
            }

            using (var client = RestHelper.Create())
            {
                var requestString = ApiMethods.AccontUserByEmail + useremail;
                var request = await client.GetAsync(requestString);
                if (request.StatusCode != HttpStatusCode.BadRequest)
                {
                    var answer = await request.Content.ReadAsAsync<ApplicationUserDTO>();
                    Session["User"] = answer;
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }


    public abstract class SocialProvider
    {
        public string AccessToken { get; set; }
        public string Code { get; set; }
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApplicationAuthPath { get; set; }
        public string ApplicationAuthTokenPath { get; set; }
        public string RequestUserInfoPath { get; set; }

        public virtual string RequestAuth
        {
            get
            {
                var uriBuilder = new UriBuilder(this.ApplicationAuthPath);
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["response_type"] = "code";
                parameters["client_id"] = this.ClientId;
                parameters["redirect_uri"] = this.RedirectUri;
                parameters["scope"] = "openid%20profile%20email";
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



    }

    public class GoogleAuth : SocialProvider
    {
        public GoogleAuth()
        {
            this.RedirectUri = "https://localhost:44366/ExternalLogin/GoogleCode";
            this.ClientId = "803183701728-q1ktbmuhces4vdj9udkmatn0gota8he8.apps.googleusercontent.com";
            this.ClientSecret = "yXNNeyD0OlL7pS-yfSzGL4bv";
            this.ApplicationAuthPath = "https://accounts.google.com/o/oauth2/auth";
            this.ApplicationAuthTokenPath = "https://accounts.google.com/o/oauth2/token";
            this.RequestUserInfoPath = "https://www.googleapis.com/oauth2/v1/userinfo";
        }

        public string ProviderName()
        {
            return "Google";
        }
    }

}