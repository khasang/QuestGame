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
            ViewBag.Title = "Список доступных квестов";
            IEnumerable<QuestViewModel> questsVM = new List<QuestViewModel>();
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.GetAsync(@"api/Quest/GetByUser");
                    request.EnsureSuccessStatusCode();

                    var quests = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();
                    questsVM = mapper.Map<IEnumerable<QuestDTO>, IEnumerable<QuestViewModel>>(quests);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return View(questsVM);
            }
        }

        // GET: Design/Quests/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.Title = "Details";
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.GetAsync(@"api/Quest/GetById?id=" + id);
                    request.EnsureSuccessStatusCode();

                    var quest = await request.Content.ReadAsAsync<QuestDTO>();
                    var questVM = mapper.Map<QuestDTO, QuestViewModel>(quest);
                    return View(questVM);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return View();
                }
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["User"] as UserModel;

            var quest = mapper.Map<QuestViewModel, QuestDTO>(model);
            //quest.Owner = user.UserName;
            quest.Date = DateTime.Now;

            if (model.File != null)
            {
                string fileName = System.IO.Path.GetFileName(model.File.FileName);
                quest.Image = fileName;
            }

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.PostJsonAsync(@"api/Quest/Add", quest);
                    request.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return RedirectToAction("Index");
            }
        }

        // GET: Design/Quests/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.GetAsync(@"api/Quest/GetById?id=" + id);
                    request.EnsureSuccessStatusCode();

                    var quest = await request.Content.ReadAsAsync<QuestDTO>();
                    var questVM = mapper.Map<QuestDTO, QuestViewModel>(quest);

                    return View(questVM);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        // POST: Design/Quests/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(QuestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["User"] as UserModel;
            var quest = mapper.Map<QuestViewModel, QuestDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.PutJsonAsync(@"api/Quest/Update", quest);
                    request.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Design/Quests/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var request = await client.DeleteAsync(@"api/Quest/Delete?id=" + id);
                    request.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }

    }
}
