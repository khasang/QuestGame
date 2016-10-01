using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Controllers;
using QuestGame.WebMVC.Areas.Game.Models;
using QuestGame.WebMVC.Areas.Design.Models;

namespace QuestGame.WebMVC.Areas.Design.Controllers
{
    public class StageController : BaseController
    {
        public StageController(IMapper mapper)
            : base(mapper)
        { }

        public async Task<ActionResult> Index(int? id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.StageGetByQuestId + id);

                IEnumerable<StageViewModel> model = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.BadRequest;
                }
                else
                {
                    var answer = await response.Content.ReadAsAsync<IEnumerable<StageDTO>>();
                    model = mapper.Map<IEnumerable<StageDTO>, IEnumerable<StageViewModel>>(answer);
                }

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            ViewBag.QuestId = id;
            return View(new NewItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewItemViewModel model, int id)
        {
            if (!ModelState.IsValid)
                return View(id);

            var stage = mapper.Map<NewItemViewModel, StageDTO>(model);
            stage.QuestId = id;
            stage.Body = string.Empty;

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PostAsJsonAsync(ApiMethods.StageCreate, stage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotCreate;
                    return View(model);
                }
            }

            return RedirectToAction("Edit", "Quest", new { id = id });
        }

        public async Task<ActionResult> Details(int id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var stageResponse = await client.GetAsync(ApiMethods.StageGetById + id);
                if (stageResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.StageNotFound;
                    return RedirectToAction("Details", "Quest");
                }
                var stageAnswer = await stageResponse.Content.ReadAsAsync<StageDTO>();
                var stageModel = mapper.Map<StageDTO, StageViewModel>(stageAnswer);

                var motionResponse = await client.GetAsync(ApiMethods.MotionGetByStageId + id);
                if (motionResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotFound;
                    return RedirectToAction("Index", "Quest");
                }
                var answer = await motionResponse.Content.ReadAsAsync<IEnumerable<MotionDTO>>();
                var model = mapper.Map<IEnumerable<MotionDTO>, IEnumerable<MotionViewModel>>(answer);

                stageModel.Motions = model.ToDictionary(x => x.Id, y => y.Description);

                return View(stageModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = ErrorMessages.StageNotFound;
                return RedirectToAction("Index", "Designer");
            }

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var stageResponse = await client.GetAsync(ApiMethods.StageGetById + id);
                if (stageResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.StageNotFound;
                    return RedirectToAction("Edit", "Quest", id);
                }
                var stageDTO = await stageResponse.Content.ReadAsAsync<StageDTO>();
                var stageModel = mapper.Map<StageDTO, StageViewModel>(stageDTO);

                var motionResponse = await client.GetAsync(ApiMethods.MotionGetByStageId + id);
                if (motionResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotFound;
                    return RedirectToAction("Index", "Designer");
                }
                var motionDTO = await motionResponse.Content.ReadAsAsync<IEnumerable<MotionDTO>>();
                var motionModel = mapper.Map<IEnumerable<MotionDTO>, IEnumerable<MotionViewModel>>(motionDTO);

                stageModel.Motions = motionModel.ToDictionary(x => x.Id, y => y.Description);

                ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer.AbsolutePath;
                return View(stageModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StageViewModel quest, string returnUrl)
        {
            //if(!ModelState.IsValid)
            //{
            //    ViewBag.ReturnUrl = returnUrl;
            //    return View(quest);
            //}

            var model = mapper.Map<StageViewModel, StageDTO>(quest);

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PutAsJsonAsync(ApiMethods.StageUpdate, model);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.StageNotUpdate;
                }
            }

            return RedirectToAction("Details", "Stage", new { id = model.Id });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return View(id);

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.StageGetById + id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.StageNotFound;
                }
                var answer = await response.Content.ReadAsAsync<StageDTO>();
                var quests = mapper.Map<StageDTO, StageViewModel>(answer);

                ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer.AbsolutePath;
                return View(quests);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmDelete(int id, string returnUrl)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.DeleteAsync(ApiMethods.StageDelById + id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.StageNotDelete;
                }
            }

            return RedirectToLocal(returnUrl);
        }
    }
}