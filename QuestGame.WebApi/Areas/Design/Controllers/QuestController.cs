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
using QuestGame.WebApi.Constants;
using QuestGame.WebApi.Areas.Design.Models;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class QuestController : BaseController
    {
        public QuestController(IMapper mapper)
            : base(mapper)
        { }

        // GET: Game/Designer
        public async Task<ActionResult> Index()
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.QuestGetUserName + SessionUser.UserName);

                IEnumerable<QuestViewModel> model = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.BadRequest;
                }
                else
                {
                    var answer = await response.Content.ReadAsAsync<IEnumerable<QuestDTO>>();
                    model = mapper.Map<IEnumerable<QuestDTO>, IEnumerable<QuestViewModel>>(answer);
                }                                      
                
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new NewItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewItemViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = mapper.Map<NewItemViewModel, QuestDTO>(model);
            request.Owner = SessionUser.UserName;

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PostAsJsonAsync(ApiMethods.QuestFullCreate, request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotCreate;
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Quest");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return View(id);

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.QuestGetById + id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotFound;
                }
                var answer = await response.Content.ReadAsAsync<QuestDTO>();
                var quests = mapper.Map<QuestDTO, QuestViewModel>(answer);

                return View(quests);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmDelete(int id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.DeleteAsync(ApiMethods.QuestFullDelById + id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotDelete;
                }
            }

            return RedirectToAction("Index", "Quest");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = "Не удалось найти квест!";
                return RedirectToAction("Index", "Designer");
            }

            using(var client = RestHelper.Create(SessionUser.Token))
            {
                var questResponse = await client.GetAsync(ApiMethods.QuestGetById + id);
                if (questResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotFound;
                    return RedirectToAction("Index", "Designer");
                }
                var questDTO = await questResponse.Content.ReadAsAsync<QuestDTO>();
                var questModel = mapper.Map<QuestDTO, QuestViewModel>(questDTO);

                var stageResponse = await client.GetAsync(ApiMethods.StageGetByQuestId + id);
                if (stageResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotFound;
                    return RedirectToAction("Index", "Designer");
                }
                var stagesDTO = await stageResponse.Content.ReadAsAsync<IEnumerable<StageDTO>>();
                var stagesModel = mapper.Map<IEnumerable<StageDTO>, IEnumerable<StageViewModel>>(stagesDTO);

                questModel.Stages = stagesModel.ToDictionary(x => x.Id, y => y.Title);

                ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer.AbsolutePath;
                return View(questModel);
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Edit(QuestViewModel quest, string returnUrl)
        {
            //if(!ModelState.IsValid)
            //{
            //    ViewBag.ReturnUrl = returnUrl;
            //    return View(quest);
            //}

            var model = mapper.Map<QuestViewModel, QuestDTO>(quest);

            using(var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PutAsJsonAsync(ApiMethods.QuestUpdate, model);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotUdate;
                    return RedirectToAction("Index", "Quest");
                }
            }

            return RedirectToAction("Details", "Quest", new { id = quest.Id });
        }

        public async Task<ActionResult> Details(int id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var questResponse = await client.GetAsync(ApiMethods.QuestGetById + id);
                if (questResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotFound;
                    return RedirectToAction("Index", "Quest");
                }
                var questAnswer = await questResponse.Content.ReadAsAsync<QuestDTO>();
                var questModel = mapper.Map<QuestDTO, QuestViewModel>(questAnswer);

                var stageResponse = await client.GetAsync(ApiMethods.StageGetByQuestId + id);
                if (stageResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotFound;
                    return RedirectToAction("Index", "Quest");
                }
                var answer = await stageResponse.Content.ReadAsAsync<IEnumerable<StageDTO>>();
                var model = mapper.Map<IEnumerable<StageDTO>, IEnumerable<StageViewModel>>(answer);

                questModel.Stages = model.ToDictionary(x => x.Id, y => y.Title);

                return View(questModel);
            }
        }
    }
}