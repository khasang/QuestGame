using System;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Game.Models;
using QuestGame.WebApi.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;

namespace QuestGame.WebApi.Areas.Game.Controllers
{
    public class DesignerController : BaseController
    {
        public DesignerController(IMapper mapper)
            : base(mapper)
        { }

        // GET: Game/Designer
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", SessionUser.Token);

                var response = await client.GetAsync(@"api/Quest/GetAll");

                IEnumerable<QuestViewModel> model = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Неудачный запрос!";
                }
                else
                {
                    var answer = await response.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

                    var quests = mapper.Map<IEnumerable<QuestDTO>, IEnumerable<QuestViewModel>>(answer);
                    model = quests.Where(x => x.Owner == SessionUser.UserName);
                }                                      
                
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult AddQuest()
        {
            var model = new NewQuestViewModel
            {
                Title = "Title",
                Owner = SessionUser.UserName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuest(NewQuestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = mapper.Map<NewQuestViewModel, QuestDTO>(model);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", SessionUser.Token);

                var response = await client.PostAsJsonAsync(@"api/Quest/Add", request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Не удалось добавить квест!";
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Designer");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteQuest(string title)
        {
            if (title == null)
                return View(title);

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", SessionUser.Token);

                var response = await client.DeleteAsync(@"api/Quest/DelByTitle?title=" + title);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Не удалось удалить квест!";
                }
            }

            return RedirectToAction("Index", "Designer");
        }

        [HttpGet]
        public async Task<ActionResult> Details(string title)      
        {
            if (title == null)
                return View(title);

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicatGion/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", SessionUser.Token);

                var response = await client.GetAsync(@"api/Quest/Details?title=" + title);

                //var response = await client.DeleteAsync(@"api/Quest/DelByTitle?title=" + title);

                //if (response.StatusCode != HttpStatusCode.OK)
                //{
                //    ViewBag.Message = "Не удалось удалить квест!";
                //}
            }

            return View();
        }
    }
}