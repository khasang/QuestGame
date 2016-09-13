using AutoMapper;
using QuestGame.Common;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Design.Models;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var model = new MotionViewModel();

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.GetAsync<MotionDTO>(@"api/Stage/GetById?id=" + id);
                    model.OwnerStageId = response.Id;
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Stages", new { id = id});
            }
        }

        // POST: Design/Motions/Create
        [HttpPost]
        public async Task<ActionResult> Create(MotionViewModel model)
        {
            var user = Session["User"] as UserModel;

            model.Id = 0;

            var motion = mapper.Map<MotionViewModel, MotionDTO>(model);

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.PostJsonAsync(@"api/Motion/Add", motion);
                }

                return RedirectToAction("Details", "Stage", new { id = model.OwnerStageId });
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Design/Motions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Design/Motions/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Design/Motions/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quest = await client.DeleteAsync(@"api/Motion/Delete?id=" + id);
            }

            return RedirectToAction("Index", "Quests");
        }
    }
}
