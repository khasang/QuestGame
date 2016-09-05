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
        public async Task<ActionResult> Login( UserLoginVM user )
        {
            string token;
            ApplicationUser UserInfo = new ApplicationUser();


            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:9243");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsJsonAsync("api/Account/LoginUser", user);

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ViewBag.ErrorMessage = "Неудачная попытка аутентификации!";
                        return View();
                    }

                    token = await response.Content.ReadAsStringAsync();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var requestParams = new Dictionary<string, string>
                    {
                        { "Email", user.Email },
                        { "Password", user.Password }
                    };

                    var content = new FormUrlEncodedContent(requestParams);

                    var responseProfile = await client.PostAsync("api/Account/UserProfile", content);

                    if (responseProfile.StatusCode == HttpStatusCode.OK)
                    {
                        UserInfo = await responseProfile.Content.ReadAsAsync<ApplicationUser>();

                        UserInfo.Token = token;
                    }

                    Session["UserInfo"] = UserInfo;


                }
            }

            return RedirectToAction("Test");
        }

        public ActionResult Logout()
        {

            Session["UserInfo"] = null;

            return View();
        }

        public ActionResult Test()
        {

            Console.WriteLine(User.Identity.IsAuthenticated);

            return View();
        }
    }
}
