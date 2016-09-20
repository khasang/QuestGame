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
    public class QuestsController : Controller
    {
        IMapper mapper;

        public QuestsController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // GET: Design/Quests
        public async Task<ActionResult> Index()
        {
            IEnumerable<QuestViewModel> questsVM = new List<QuestViewModel>();
            ViewBag.Title = "Список доступных квестов";
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quests = await client.GetAsync<IEnumerable<QuestDTO>>(@"api/Quest/GetByUser");

                questsVM = mapper.Map<IEnumerable<QuestDTO>, IEnumerable<QuestViewModel>>(quests);
                return View(questsVM);
            }
        }

        // GET: Design/Quests/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.Title = "Details";

            ViewBag.Alert = Session["SomeErrors"];

            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quest = await client.GetAsync<QuestDTO>(@"api/Quest/GetById?id=" + id);
                var questVM = mapper.Map<QuestDTO, QuestViewModel>(quest);

                return View(questVM);
            }
        }

        // GET: Design/Quests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Design/Quests/Create
        [HttpPost]
        public async Task<ActionResult> Create(QuestViewModel model)
        {
            var user = Session["User"] as UserModel;

            var quest = mapper.Map<QuestViewModel, QuestDTO>(model);
            quest.Owner = user.UserName;
            quest.Date = DateTime.Now;

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.PostJsonAsync(@"api/Quest/Add", quest);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Design/Quests/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quest = await client.GetAsync<QuestDTO>(@"api/Quest/GetById?id=" + id);
                var questVM = mapper.Map<QuestDTO, QuestViewModel>(quest);

                return View(questVM);
            }
        }

        // POST: Design/Quests/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(QuestViewModel model)
        {

            var user = Session["User"] as UserModel;
            var quest = mapper.Map<QuestViewModel, QuestDTO>(model);

            try
            {
                using (var client = new RequestApi(user.Token))
                {
                    var response = await client.PutJsonAsync(@"api/Quest/Update", quest);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


        // GET: Design/Quests/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quest = await client.DeleteAsync(@"api/Quest/Delete?id=" + id);
            }

            return RedirectToAction("Index");
        }

    }
}
