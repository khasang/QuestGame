using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Attributes;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Helpers.SocialProviders;
using QuestGame.WebMVC.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http.Headers;


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
        public ActionResult GoogleAuthCallback(string code)
        {
            SocialProvider provider = new GoogleAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            return GetSocialUser(userInfo).Result;
        }

        [HttpGet]
        public ActionResult YandexAuthCallback(string code)
        {
            SocialProvider provider = new YandexAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            return GetSocialUser(userInfo).Result;
        }

        [HttpGet]
        public ActionResult FaceBookAuthCallback(string code)
        {
            SocialProvider provider = new FacebookAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            return GetSocialUser(userInfo).Result;
        }

        [HttpGet]
        public ActionResult VKontakteAuthCallback(string code)
        {
            SocialProvider provider = new VKontakteAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            return GetSocialUser(userInfo).Result;
        }

        [HttpGet]
        [HTTPExceptionAttribute]
        public async Task<ActionResult> GetSocialUser(SocialUserModel model)
        {
            var user = mapper.Map<SocialUserModel, SocialUserDTO>(model);

            using (var client = RestHelper.Create())
            {
                var response = client.GetAsync(ApiMethods.AccontUserByEmail + user.Email).Result;

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return CreateSocialUser(model).Result;
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsAsync<ApplicationUserDTO>();

                if (result.Logins.Count == 0 )
                {
                    ViewBag.Title = "Ошибка";
                    ViewBag.Message = "Пользователь с email указанным соц. сетью уже существует";

                    return View("ActionResultInfo");
                }

                Session["User"] = result;

                return RedirectToAction("Index", "Home");
            }
        }

        [HTTPExceptionAttribute]
        public async Task<ActionResult> CreateSocialUser(SocialUserModel model)
        {
            SocialUserDTO user = mapper.Map<SocialUserModel, SocialUserDTO>(model);

            using (var client = RestHelper.Create())
            {
                var response = client.PostAsJsonAsync(@"api/Account/RegisterSocialUser", user).Result;
                var answer = await response.Content.ReadAsAsync<ApplicationUserDTO>();

                Session["User"] = answer;

                return RedirectToAction("Index", "Home");
            }
        }

    }
}