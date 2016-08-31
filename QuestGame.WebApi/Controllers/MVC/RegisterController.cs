using System.Web.Mvc;
using QuestGame.WebApi.Models;
using System.Threading.Tasks;
using Serilog;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;

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
            ViewBag.Title = "Страница регистрации";

            return View( new UserInvite() );
        }

        [HttpPost]
        public async Task<ActionResult> Index( UserInvite user )
        {
            IRequest client = new DirectRequest();
            var response = await client.PostRequestAsync(@"api/Account/Register", user);

            if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Успешная регистрация";

                    myLogger.Information("Регистрация пользователя");
                }
                else
                {
                    ViewBag.Message = "Что-то пошло не так";
                }

            return View("CreateUser");
        }
    }
}
