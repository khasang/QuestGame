using QuestGame.Common;
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
                View(model);
            }

            using (var client = new RequestApi())
            {
                var response = await client.PostJsonAsync(@"api/Account/Register", model);
                var answer = await response.Content.ReadAsAsync<RegisterResponse>();

                ViewBag.ErrorMessage = answer.Success ? "Пользователь успешно зарегистрирован!" : "Ошибка регистрации!";

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

            using (var client = new RequestApi())
            {
                var response = await client.PostJsonAsync(@"api/Account/LoginUser", model);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var answer = await response.Content.ReadAsStringAsync();

                    //Записать токен в сесию
                    Session["User"] = new UserModel { UserName = model.Email, Token = answer };

                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage = "Неудачная попытка аутентификации!";
                return View();
            }
        }

        public ActionResult LogOff()
        {
            Session["User"] = null;
            return View("Index");
        }
    }    
}
