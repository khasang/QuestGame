using AutoMapper;
using QuestGame.Common;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Design.Models;
using QuestGame.WebApi.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class StageController : Controller
    {
        IMapper mapper;

        public StageController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // GET: Design/Stage
        public ActionResult Index()
        {
            return View();
        }

        //// GET: Design/Stage/Details/5
        //public async Task<ActionResult> Details(int id)
        //{
        //    ViewBag.Title = "Details";

        //    var user = Session["User"] as UserModel;

        //    using (var client = new RequestApi(user.Token))
        //    {
        //        var stage = await client.GetAsync<StageDTO>(@"api/Stage/GetById?id=" + id);
        //        var stageVM = mapper.Map<StageDTO, StageViewModel>(stage);

        //        return View(stageVM);
        //    }
        //}

        // GET: Design/Stage/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.Title = "Details";

            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var response = await client.GetAsyncResult<StageDTO>(@"api/Stage/GetById?id=" + id);

                if (response.ResponseErrors != null)
                {
                    var stageFake = new StageViewModel();

                    ViewBag.Alert = response.ResponseErrors;
                    return View(stageFake);
                }

                var stage = response.ResponseData;
                var stageVM = mapper.Map<StageDTO, StageViewModel>(stage);

                return View(stageVM);

            }
        }

        // GET: Design/Stage/Create
        public async Task<ActionResult> Create(int id)
        {
            var user = Session["User"] as UserModel;

            var model = new StageViewModel();

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.GetAsync<QuestDTO>(@"api/Quest/GetById?id=" + id);
                    model.QuestId = response.Id;
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }

        // POST: Design/Stage/Create
        [HttpPost]
        public async Task<ActionResult> Create(StageViewModel model)
        {
            var user = Session["User"] as UserModel;

            model.Id = 0;

            var stage = mapper.Map<StageViewModel, StageDTO>(model);

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.PostJsonAsync(@"api/Stage/Add", stage);
                }

                return RedirectToAction("Details", "Quests", new { id = model.QuestId});
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Design/Stage/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var stage = await client.GetAsync<StageDTO>(@"api/Stage/GetById?id=" + id);
                var stageVM = mapper.Map<StageDTO, StageViewModel>(stage);

                return View(stageVM);
            }
        }

        // POST: Design/Stage/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(StageViewModel model)
        {
            var user = Session["User"] as UserModel;
            var stage = mapper.Map<StageViewModel, StageDTO>(model);

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.PutJsonAsync(@"api/Stage/Update", stage);
                }

                return RedirectToAction("Details", "Quests", new { id = stage.QuestId});
            }
            catch
            {
                return View(model);
            }
        }

        // POST: Design/Stage/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var response = await client.DeleteAsync(@"api/Stage/Delete?id=" + id);
            }

            return RedirectToAction("Index", "Quests");
        }
    }
}
