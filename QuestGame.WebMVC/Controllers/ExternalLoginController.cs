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
        // GET: ExternalLogin
        public ActionResult Index()
        {
            SocialProvider provider = new YandexAuth();
            //SocialProvider provider = new GoogleAuth();
            //SocialProvider provider = new FacebookAuth();


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


        private async Task GetSocialUser(SocialUserModel socialUser)
        {
            using (var client = RestHelper.Create())
            {
                var requestString = ApiMethods.AccontUserByEmail + socialUser.Email;
                var request = await client.GetAsync(requestString);
                if (request.StatusCode != HttpStatusCode.BadRequest)
                {
                    var answer = await request.Content.ReadAsAsync<ApplicationUserDTO>();
                    Session["User"] = answer;
                }
                else
                {
                    RedirectToAction("CreateSocialUser", new { socialUser = socialUser });
                }
            }
        }

        //public async Task<ActionResult> CreateSocialUser(SocialUserModel socialUser)
        //{
        //    var ussser = new RegisterExternalBindingModel {
        //        Email = socialUser.Email
        //    };

        //    using (var client = RestHelper.Create())
        //    {
        //        var response = await client.PostAsJsonAsync(@"api/Account/RegisterExternal", ussser);
        //    }

        //    return View();
        //}

        //public async Task<ActionResult> ExternalLoginConfirmation()
        //{
        //    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //    var result = await UserManager.CreateAsync(user);
        //    if (result.Succeeded)
        //    {
        //        result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //            return RedirectToLocal(returnUrl);
        //        }
        //    }
        //}
    }
}