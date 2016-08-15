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

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Авторизация";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login( UserLogin user )
        {

            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:9243");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var requestParams = new Dictionary<string, string>
                    {
                        { "grant_type", "password" },
                        { "username", user.Email },
                        { "password", user.Password }
                    };

                    var content = new FormUrlEncodedContent(requestParams);
                    var response = await client.PostAsync("Token", content);

                    var responseData = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    var authToken = responseData["access_token"];

                    Session["Token"] = authToken;

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Авторизация пройдена";

                        myLogger.Information("Новая авторизация пользователя");
                    }
                    else
                    {
                        ViewBag.Message = "Не авторизирован";
                    }
                }
            }

            return RedirectToAction("Test", Session["Token"]);

            //return View("");
        }


        [Authorize]
        public ActionResult Test()
        {
            return View();
        }
    }
}
