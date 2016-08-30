using QuestGame.Domain.Entities;
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9243");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(@"api/Quests");
                var responseData = await response.Content.ReadAsAsync<IEnumerable<Quest>>();

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Quests = responseData.OrderByDescending(q => q.AddDate);
                }
                else
                {
                    ViewBag.Message = "Что-то пошло не так";
                }
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

            return null;
        }
    }
}