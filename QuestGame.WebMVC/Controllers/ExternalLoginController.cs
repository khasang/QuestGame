using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Helpers.SocialProviderFactory;
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
        SocialProvider provider;

        public ActionResult Index()
        {
            this.provider = SocialProviderCreator.Create("GoogleAuth");

            return Redirect(provider.RequestAuth);
        }

        [HttpGet]
        public async Task<ActionResult> GoogleCode(string code)
        {
                using (var client = new HttpClient())
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
                    var response = await client.PostAsync("https://accounts.google.com/o/oauth2/token", content);

                    var responseData = await response.Content.ReadAsAsync<Dictionary<string, string>>();

                    return RedirectToAction("GoogleUser", new { token = responseData["access_token"] });

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


}