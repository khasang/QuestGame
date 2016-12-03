using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Attributes;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Helpers.SocialProviders;
using QuestGame.WebMVC.Models;

namespace QuestGame.WebMVC.Controllers
{
    public class ExternalLoginController : BaseController
    {
        public ExternalLoginController(IMapper mapper)
            : base(mapper)
        { }

        // GET: ExternalLogin
        public ActionResult Index(string providerName)
        {
            var provider = GetProvider.CreateProvider(providerName);

            return Redirect(provider.RequestCodeUrl);
        }

        [HttpGet]
        [HTTPException]
        public async Task<ActionResult> GoogleAuthCallback(string code)
        {
            var provider = new GoogleAuth
            {
                Code = code
            };

            var userInfo = provider.GetUserInfo();

            return await GetSocialUser(userInfo);
        }

        [HttpGet]
        [HTTPException]
        public async Task<ActionResult> YandexAuthCallback(string code)
        {
            var provider = new YandexAuth
            {
                Code = code
            };

            var userInfo = provider.GetUserInfo();

            return await GetSocialUser(userInfo);
        }

        [HttpGet]
        [HTTPException]
        public async Task<ActionResult> FaceBookAuthCallback(string code)
        {
            var provider = new FacebookAuth
            {
                Code = code
            };

            var userInfo = provider.GetUserInfo();

            return await GetSocialUser(userInfo);
        }

        [HttpGet]
        [HTTPException]
        public async Task<ActionResult> VKontakteAuthCallback(string code)
        {
            var provider = new VKontakteAuth
            {
                Code = code
            };

            var userInfo = provider.GetUserInfo();

            return await GetSocialUser(userInfo);
        }
        
        private async Task<ActionResult> GetSocialUser(SocialUserModel model)
        {
            var user = mapper.Map<SocialUserModel, SocialUserDTO>(model);

            using (var client = RestHelper.Create())
            {
                var response = await client.GetAsync(ApiMethods.AccontUserByEmail + user.Email);
                response.EnsureSuccessStatusCode();

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return await CreateSocialUser(model);
                }

                var result = await response.Content.ReadAsAsync<ApplicationUserDTO>();

                var isSelf = result.Logins.Any(s => s.Equals(model.Provider));

                if (!isSelf)
                {
                    ViewBag.Title = ErrorMessages.AccountFailRegister;
                    ViewBag.Message = ErrorMessages.AccountUserExist;

                    return View("ActionResultInfo");
                }

                var authrequest = await client.PostAsJsonAsync(ApiMethods.AccontUserGetSocial, user);
                var answer = await authrequest.Content.ReadAsAsync<ApplicationUserDTO>();

                Session["User"] = answer;

                return RedirectToAction("Index", "Home");
            }
        }
        
        private async Task<ActionResult> CreateSocialUser(SocialUserModel userModel)
        {
            var user = mapper.Map<SocialUserModel, SocialUserDTO>(userModel);

            ApplicationUserDTO userResult;

            using (var client = RestHelper.Create())
            {
                var response = await client.PostAsJsonAsync(ApiMethods.AccontUserSocialRegister, user);
                userResult = await response.Content.ReadAsAsync<ApplicationUserDTO>();

                Session["User"] = userResult;
            }

            using (var clientUpdate = RestHelper.Create(SessionUser.Token))
            {

                var filePath = await UploadFile(user.AvatarUrl, ApiMethods.AccountUploadFile);
                if (!string.IsNullOrEmpty(filePath))
                    userResult.UserProfile.AvatarUrl = filePath;

                var responseUpdate = await clientUpdate.PostAsJsonAsync(ApiMethods.AccontEditUser, userResult);

                return RedirectToAction("UserProfile", "Account", new { id = userResult.Id });
            }
        }
    }
}