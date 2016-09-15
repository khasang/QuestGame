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
using QuestGame.Common.Helpers;
using System.Collections.Specialized;

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
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(@"api/QuestFull/GetAll");

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
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PostAsJsonAsync(@"api/QuestFull/Add", request);
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

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.DeleteAsync(@"api/QuestFull/DelByTitle?title=" + title);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Не удалось добавить квест!";
                }
            }

            return RedirectToAction("Index", "Designer");
        }

        [HttpGet]
        public async Task<ActionResult> EditQuest(int id)
        {
            using(var client = RestHelper.Create(SessionUser.Token))
            {
                var questResponse = await client.GetAsync(@"api/Quest/GetById?id=" + id);
                if (questResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Не удалось найти квест!";
                    return RedirectToAction("Index", "Designer");
                }
                var questDTO = await questResponse.Content.ReadAsAsync<QuestDTO>();
                var questModel = mapper.Map<QuestDTO, QuestViewModel>(questDTO);

                var stageResponse = await client.GetAsync(@"api/Stage/GetByQuestId?id=" + id);
                if (stageResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Не удалось найти квест!";
                    return RedirectToAction("Index", "Designer");
                }
                var stagesDTO = await stageResponse.Content.ReadAsAsync<IEnumerable<StageDTO>>();
                var stagesModel = mapper.Map<IEnumerable<StageDTO>, IEnumerable<StageViewModel>>(stagesDTO);

                questModel.Stages = stagesModel.ToDictionary(x => x.Id, y => y.Title);

                return View(questModel);
            }            
        }

        [HttpPost]
        public async Task<ActionResult> EditQuest(QuestViewModel quest)
        {
            if(!ModelState.IsValid)
            {
                return View(quest);
            }

            var model = mapper.Map<QuestViewModel, QuestDTO>(quest);

            using(var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PutAsJsonAsync(@"api/Quest/Update", model);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Не обновить квест!";
                    return RedirectToAction("Index", "Designer");
                }
            }

            return RedirectToAction("Index");
        }
    }
}