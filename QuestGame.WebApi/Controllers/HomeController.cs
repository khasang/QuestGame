using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Constants;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QuestGame.WebApi.Controllers
{
    public class HomeController : Controller
    {
        IMapper mapper;

        public HomeController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = RestHelper.Create())
            {
                var response = await client.PostAsJsonAsync(@"api/Account/Register", model);
                var answer = await response.Content.ReadAsAsync<RegisterResponse>();

                if (answer.Success)
                {
                    ViewBag.ErrorMessage = "Пользователь успешно зарегистрирован!";
                }
                else
                {
                    ViewBag.ErrorMessage = "Ошибка регистрации!";
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                View(model);
            }

            using (var client = RestHelper.Create())
            {
                var response = await client.PostAsJsonAsync(@"api/Account/LoginUser", model);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Неудачная попытка аутентификации!";
                    return View();
                }

                var answer = await response.Content.ReadAsStringAsync();

                //Записать токен в сесию
                Session["User"] = new UserModel { UserName = model.Email, Token = answer };

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> UserInfo(string name)
        {
            using (var client = RestHelper.Create())
            {
                var response = await client.GetAsync(ApiMethods.UserGetByName + name);
                var answer = await response.Content.ReadAsAsync<UserDTO>();

                var model = mapper.Map<UserDTO, UserViewModel>(answer);

                model.UserProfile.avatarUrl = "http://vignette3.wikia.nocookie.net/shokugekinosoma/images/6/60/No_Image_Available.png/revision/latest?cb=20150708082716";

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserInfoEdit(UserViewModel model)
        {
            var user = mapper.Map<UserViewModel, UserDTO>(model);

            var currentUser = Session["User"] as UserModel;

            using (var client = RestHelper.Create(currentUser.Token))
            {
                var response = await client.PostAsJsonAsync(@"api/Account/EditUserByName", user);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Неудачная попытка аутентификации!";
                }

                return RedirectToAction("UserInfo", new { name = model.UserName });
            }
        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            return View("Index");
        }
    }
}
