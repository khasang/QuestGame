using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Helpers.SocialProviders;
using QuestGame.WebMVC.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace QuestGame.WebMVC.Controllers
{
    public class ExternalLoginController : Controller
    {
        IMapper mapper;

        public ExternalLoginController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // GET: ExternalLogin
        public ActionResult Index(string providerName)
        {
            var provider = GetProvider.Provider(providerName);

            return Redirect(provider.RequestCodeUrl);
        }

        [HttpGet]
        public async Task<ActionResult> GoogleAuthCallback(string code)
        {
            SocialProvider provider = new GoogleAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            await GetSocialUser(userInfo);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> YandexAuthCallback(string code)
        {
            SocialProvider provider = new YandexAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            await GetSocialUser(userInfo);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> FaceBookAuthCallback(string code)
        {
            SocialProvider provider = new FacebookAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            await GetSocialUser(userInfo);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> VKontakteAuthCallback(string code)
        {
            SocialProvider provider = new VKontakteAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            await GetSocialUser(userInfo);

            return RedirectToAction("Index", "Home");
        }

        private async Task GetSocialUser(SocialUserModel socialUser)
        {
            SocialUserDTO user = mapper.Map<SocialUserModel, SocialUserDTO>(socialUser);

            using (var client = RestHelper.Create())
            {
                var request = await client.PostAsJsonAsync(@"api/Account/GetSocialUser", user);

                if (request.StatusCode == HttpStatusCode.NotFound)
                {
                    await CreateSocialUser(socialUser);
                }
                else if (request.StatusCode == HttpStatusCode.OK)
                {
                    var answer = await request.Content.ReadAsAsync<ApplicationUserDTO>();
                    Session["User"] = answer;
                }
            }
        }

        public async Task CreateSocialUser(SocialUserModel socialUser)
        {
            SocialUserDTO user = mapper.Map<SocialUserModel, SocialUserDTO>(socialUser);

            using (var client = RestHelper.Create())
            {
                
                var response = await client.PostAsJsonAsync(@"api/Account/RegisterSocialUser", user);

                var answer = await response.Content.ReadAsAsync<ApplicationUserDTO>();

                Session["User"] = answer;
            }
        }

    }
}