using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using QuestGame.WebApi.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Serilog;
using System.Security.Claims;
using System.Threading;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using System.Net;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Models.UserViewModels;
using QuestGame.Common;

namespace QuestGame.WebApi.Controllers
{
    public class HomeController : Controller
    {
        ILogger myLogger = null;

        public HomeController()
        {
            myLogger = Log.Logger = new LoggerConfiguration()
                                        .WriteTo.RollingFile(@"e:\myapp-Log.txt")
                                        .CreateLogger();
        }

        public async Task<ActionResult> Index()
        {
            IRequest client = new DirectRequest();
            var response = await client.GetRequestAsync(@"api/Quests");
            var responseData = await response.Content.ReadAsAsync<IEnumerable<Quest>>();

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Quests = responseData.OrderByDescending(q => q.AddDate).Take(5);
            }
            else
            {
                ViewBag.Message = "Что-то пошло не так";
            }


            //using (var db = new QuestGame.Domain.ApplicationDbContext())
            //{
            //    ViewBag.Users = db.Users.OrderByDescending(u => u.AddDate).Select(u => u.UserName).ToList().Take(5);
            //}

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Авторизация";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginVM user)
        {
            ViewBag.Title = "Авторизация";

            string token;
            var UserInfo = new UserProfileVM();


            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login", user);
            }

            using (var client = new RequestApi())
            {
                var response = await client.PostJsonAsync(@"api/Account/LoginUser", user);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.Alerts = await response.Content.ReadAsStringAsync();
                    return View();
                }
                token = await response.Content.ReadAsStringAsync();
            }

            using (var client = new RequestApi(token))
            {
                client.AddSendParam("Email", user.Email);
                client.AddSendParam("Password", user.Password);

                var responseProfile = await client.PostAsync(@"api/Account/UserProfile");

                if (responseProfile.StatusCode == HttpStatusCode.OK)
                {
                    UserInfo = await responseProfile.Content.ReadAsAsync<UserProfileVM>();

                    UserInfo.Token = token;

                    Session["UserInfo"] = UserInfo;
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {

            Session["UserInfo"] = null;

            return RedirectToAction("Index");
        }


    }
}
