using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
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
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Areas.Game.Models;
using QuestGame.WebMVC.Controllers;
using QuestGame.WebMVC.Areas.Design.Models;

namespace QuestGame.WebMVC.Areas.Design.Controllers
{
    public class MotionController : BaseController
    {
        public MotionController(IMapper mapper)
            : base(mapper)
        { }

        public async Task<ActionResult> Index(int? id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.MotionGetByStageId + id);

                IEnumerable<MotionViewModel> model = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.BadRequest;
                }
                else
                {
                    var answer = await response.Content.ReadAsAsync<IEnumerable<MotionDTO>>();
                    model = mapper.Map<IEnumerable<MotionDTO>, IEnumerable<MotionViewModel>>(answer);
                }

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            ViewBag.StageId = id;
            return View(new NewItemViewModel { Title = string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewItemViewModel model, int id)
        {
            if (!ModelState.IsValid)
                return View(id);

            var motion = mapper.Map<NewItemViewModel, MotionDTO>(model);
            motion.OwnerStageId = id;

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PostAsJsonAsync(ApiMethods.MotionCreate, motion);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotCreate;
                    return View(model);
                }
            }

            return RedirectToAction("Edit", "Stage", new { id = id });
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

                ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer.AbsolutePath;
                return View(motionModel);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = ErrorMessages.BadRequest;
                return RedirectToAction("Index", "Designer");
            }

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var motionResponse = await client.GetAsync(ApiMethods.MotionGetById + id);
                if (motionResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotFound;
                    return RedirectToAction("Index", "Quest");
                }
                var motionDTO = await motionResponse.Content.ReadAsAsync<MotionDTO>();
                var motionModel = mapper.Map<MotionDTO, MotionEditViewModel>(motionDTO);

                var stagesResponse = await client.GetAsync(ApiMethods.StageByMotionId + id);
                if (stagesResponse.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotFound;
                    return RedirectToAction("Index", "Quest");
                }

                var stagesDTO = await stagesResponse.Content.ReadAsAsync<IEnumerable<StageDTO>>();
                motionModel.RedirectStagesList = stagesDTO.ToDictionary(x => x.Id, y => y.Title);

                ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer.AbsolutePath;
                return View(motionModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MotionEditViewModel quest, string returnUrl)
        {
            //if(!ModelState.IsValid)
            //{
            //    ViewBag.ReturnUrl = returnUrl;
            //    return View(quest);
            //}

            var model = mapper.Map<MotionEditViewModel, MotionDTO>(quest);

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.PutAsJsonAsync(ApiMethods.MotionUpdate, model);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotUpdate;
                }
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return View(id);

            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.MotionGetById + id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotFound;
                }
                var answer = await response.Content.ReadAsAsync<MotionDTO>();
                var quests = mapper.Map<MotionDTO, MotionViewModel>(answer);

                ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer.AbsolutePath;
                return View(quests);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmDelete(int id, string returnUrl)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.DeleteAsync(ApiMethods.MotionDelById + id);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.MotionNotDelete;
                }
            }

            return RedirectToLocal(returnUrl);
        }
    }
}