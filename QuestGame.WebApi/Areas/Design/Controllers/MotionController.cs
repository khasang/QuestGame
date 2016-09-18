using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Game.Models;
using QuestGame.WebApi.Constants;
using QuestGame.WebApi.Controllers;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using QuestGame.WebApi.Areas.Design.Models;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class MotionController : BaseController
    {
        public MotionController(IMapper mapper)
            : base(mapper)
        { }

        [HttpGet]
        public ActionResult Create(int id)
        {
            ViewBag.StageId = id;
            return View(new NewItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewItemViewModel model, int id)
        {
            if (!ModelState.IsValid)
                return View(id);

            var motion = mapper.Map<NewItemViewModel, MotionDTO>(model);
            motion.StageId = id;

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PostAsJsonAsync(ApiMethods.MotionCreate, motion);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotCreate;
                    return View(model);
                }
            }

            return RedirectToAction("Edit", "Stage", id);
        }

        public async Task<ActionResult> Details(int id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var motionResponse = await client.GetAsync(ApiMethods.MotionGetById + id);
                if (motionResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotFound;
                    return RedirectToAction("Details", "Stage");
                }
                var motionAnswer = await motionResponse.Content.ReadAsAsync<MotionDTO>();
                var motionModel = mapper.Map<MotionDTO, MotionViewModel>(motionAnswer);

                return View(motionModel);
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