using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Helpers.SocialProviderFactory;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Controllers
{
    public class ExternalLoginController : Controller
    {
        SocialProvider provider;

        public ActionResult Index(string providerName)
        {
            this.provider = SocialProviderCreator.Create(providerName);

            return Redirect(provider.RequestAuth);
        }

        [HttpGet]
        public async Task<ActionResult> GoogleAuthCallback(string code)
        {
            this.provider = SocialProviderCreator.Create("GoogleAuth");
            provider.Code = code;

            var token = await GetSocialToken();
            provider.AccessToken = token;

            await GetSocialUser();

            return RedirectToAction("Index", "Home");
        }

        private async Task GetSocialUser()
        {
            var useremail = "";

            using (var client = RestHelper.Create())
            {
                var request = await client.GetAsync(this.provider.RequestUserInfo);
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
        }

        
        private async Task<string> GetSocialToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("Referer", this.provider.RedirectUri);

                var response = await client.PostAsync(this.provider.RequestTokenUrl, this.provider.RequestTokenContent);
                var responseData = await response.Content.ReadAsAsync<Dictionary<string, string>>();

                return responseData["access_token"];
            }
        }
    }


}