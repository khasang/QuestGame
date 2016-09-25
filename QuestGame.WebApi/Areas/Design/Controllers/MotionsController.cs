using AutoMapper;
using QuestGame.Common;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Design.Models;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class MotionsController : Controller
    {
        IMapper mapper;

        public MotionsController(IMapper mapper)
        {
            this.mapper = mapper;
        }


        // GET: Design/Motions
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Quests");
        }

        // GET: Design/Motions/Create
        public async Task<ActionResult> Create(int id)
        {
            var user = Session["User"] as UserModel;
            ViewBag.Errors = TempData["Errors"];

            var model = new MotionViewModel();

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var request = await client.GetAsync(@"api/Stage/GetById?id=" + id);
                    request.EnsureSuccessStatusCode();

                    var motion = await request.Content.ReadAsAsync<MotionDTO>();

                    model.OwnerStageId = motion.Id;
                    return View(model);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Stages", new { id = id });
            }
        }

        // POST: Design/Motions/Create
        [HttpPost]
        public async Task<ActionResult> Create(MotionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewBag.Errors = TempData["Errors"];

            var user = Session["User"] as UserModel;

            model.Id = 0;

            var motion = mapper.Map<MotionViewModel, MotionDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.PostJsonAsync(@"api/Motion/Add", motion);
                    request.EnsureSuccessStatusCode();

                    return RedirectToAction("Details", "Stage", new { id = model.OwnerStageId });
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return View(model);
                }
            }
        }


        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["User"] as UserModel;
            ViewBag.Errors = TempData["Errors"];

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.GetAsync(@"api/Motion/GetById?id=" + id);
                    request.EnsureSuccessStatusCode();

                    var motion = await request.Content.ReadAsAsync<MotionDTO>();
                    var motionVM = mapper.Map<MotionDTO, MotionViewModel>(motion);

                    return View(motionVM);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return RedirectToAction("Index", "Stages", new { id = id });
                }
            }
        }

        // POST: Design/Motions/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(MotionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewBag.Errors = TempData["Errors"];

            var user = Session["User"] as UserModel;
            var motion = mapper.Map<MotionViewModel, MotionDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.PutJsonAsync(@"api/Motion/Update", motion);
                    request.EnsureSuccessStatusCode();

                    return RedirectToAction("Details", "Stage", new { id = motion.OwnerStageId });
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return View(model);
                }
            }
        }

        // GET: Design/Motions/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;
            ViewBag.Errors = TempData["Errors"];

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.DeleteAsync(@"api/Motion/Delete?id=" + id);
                    request.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("Index", "Quests");
        }
    }
}
