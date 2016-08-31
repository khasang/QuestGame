using QuestGame.Domain.Entities;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Controllers.MVC
{
    public class QuestsController : Controller
    {
        // GET: UserQuests
        public async Task<ActionResult> Index()
        {
            IRequest client = new DirectRequest();
            var response = await client.GetRequestAsync(@"api/Quests");
            var responseData = await response.Content.ReadAsAsync<IEnumerable<Quest>>();

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Quests = responseData.OrderByDescending(q => q.AddDate);
            }
            else
            {
                ViewBag.Message = "Что-то пошло не так";
            }

            return View();
        }

        public ActionResult Add()
        {
            ViewBag.Message = "Добавление";
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Add ( Models.QuestVM quest )
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var client = new AuthRequest( (Session["UserInfo"] as ApplicationUser).Token );
            var response = await client.PostRequestAsync(@"api/Quests/Add", quest);

            return View("Index");
        }
    }
}