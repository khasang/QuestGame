using QuestGame.Common.Helpers;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using QuestGame.Domain.DTO;
using AutoMapper;

namespace QuestGame.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        //IMapper mapper;

        //public AccountController(IMapper mapper)
        //{
        //    this.mapper = mapper;
        //}

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
                View(model);
            }

            using (var client = RestHelper.Create())
            {
                var response = await client.PostAsJsonAsync(ApiMethods.AccontRegister, model);
                var answer = await response.Content.ReadAsAsync<RegisterResponse>();

                if (answer.Success)
                {
                    ViewBag.ErrorMessage = ErrorMessages.AccountSuccessRegister;
                }
                else
                {
                    ViewBag.ErrorMessage = ErrorMessages.AccountFailRegister;
                }

                return RedirectToAction("Index", "Home");
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
                var response = await client.PostAsJsonAsync(ApiMethods.AccountLogin, model);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = ErrorMessages.AccountFailLogin;
                    return View();
                }

                var answer = await response.Content.ReadAsStringAsync();

                //Записать токена в сесию
                Session["User"] = new UserModel { UserName = model.Email, Token = answer };

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            return View("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> UserInfo(string name)
        {
            using (var client = RestHelper.Create())
            {
                var response = await client.GetAsync(ApiMethods.AccontUser + name);
                var answer = await response.Content.ReadAsAsync<ApplicationUserViewModel>();

                //var model = mapper.Map<ApplicationUserDTO, ApplicationUserViewModel>(answer);

                return View(answer);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserInfoEdit(ApplicationUserViewModel model)
        {
            //var user = mapper.Map<ApplicationUserViewModel, ApplicationUserDTO>(model);

            var currentUser = Session["User"] as UserModel;

            using (var client = RestHelper.Create(currentUser.Token))
            {
                var response = await client.PostAsJsonAsync(ApiMethods.AccontEditUser, model);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Неудачная попытка редактирования!";
                }

                return RedirectToAction("UserInfo", new { name = model.UserName });
            }
        }
    }
}