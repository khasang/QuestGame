using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Design.Models;
using QuestGame.WebApi.Constants;
using QuestGame.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using QuestGame.WebApi.Areas.Game.Models;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class StageController : BaseController
    {
        public StageController(IMapper mapper)
            : base(mapper)
        { }

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

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PostAsJsonAsync(ApiMethods.StageCreate, stage);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.QuestNotCreate;
                    return View(model);
                }
            }

            return RedirectToAction("Edit", "Quest", id);
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

            return RedirectToLocal(returnUrl);
        }
    }
}