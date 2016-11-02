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

            await GetSocialUser(await provider.GetSocialUserEmail());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> FaceBookAuthCallback(string code)
        {
            this.provider = SocialProviderCreator.Create("FaceBookAuth");
            provider.Code = code;

            await GetSocialUser(await provider.GetSocialUserEmail());

            return RedirectToAction("Index", "Home");
        }




        private async Task GetSocialUser(string useremail)
        {
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
    }


}