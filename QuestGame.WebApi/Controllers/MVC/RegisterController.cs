using System.Web.Mvc;
using QuestGame.WebApi.Models;
using System.Threading.Tasks;
using Serilog;
using System.Net.Http;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Models.UserViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace QuestGame.WebApi.Controllers
{
    public class RegisterController : Controller
    {
        ILogger myLogger = null;

        public RegisterController()
        {
            myLogger = new LoggerConfiguration()
            .WriteTo.RollingFile("myapp-Log.txt")
            .CreateLogger();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Регистрация нового пользователя";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(UserRegisterVM user)
        {            
            if (!ModelState.IsValid)
            {
                ViewBag.Alerts = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return View(user);
            }

            if (user.File != null)
            {
                string fileName = System.IO.Path.GetFileName(user.File.FileName);
                //user.File.SaveAs(Server.MapPath("~/Content/Images/GameContent/" + fileName));
                user.Avatar = @"/Content/Images/GameContent/" + fileName;
            }
            else
            {
                user.Avatar = "http://www.novelupdates.com/img/noimagefound.jpg";
            }

            user.UserName = user.Email;

            IRequest client = new DirectRequest();
            var response = await client.PostRequestAsync(@"api/Account/Register", user);

            if (response.IsSuccessStatusCode)
            {
                if (user.File != null)
                {
                    string fileName = System.IO.Path.GetFileName(user.File.FileName);
                    user.File.SaveAs(Server.MapPath("~/Content/Images/GameContent/" + fileName));
                }
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Alerts = await response.Content.ReadAsAsync<IEnumerable<string>>();
                }

                return View(user);
            }

            return RedirectToAction("Login", "Home", new { area = "" });
        }
    }
}
