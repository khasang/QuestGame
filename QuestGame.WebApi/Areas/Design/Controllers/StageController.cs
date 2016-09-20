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
using QuestGame.Domain.Entities;
using QuestGame.WebApi.Infrastructure;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    [NotFoundException]
    public class StageController : Controller
    {
        IMapper mapper;

        public StageController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        
        // GET: Design/Stage/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.Title = "Details";
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.GetAsync(@"api/Stage/GetById?id=" + id);
                    response.EnsureSuccessStatusCode();

                    var stage = response.Content.ReadAsAsync<StageDTO>().Result;
                    var stageVM = mapper.Map<StageDTO, StageViewModel>(stage);

                    return View(stageVM);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        // GET: Design/Stage/Create
        public async Task<ActionResult> Create(int? id) // Приходит id квеста владельца
        {
            var user = Session["User"] as UserModel;
            var model = new StageViewModel();

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.GetAsync(@"api/Quest/GetById?id=" + id);
                    response.EnsureSuccessStatusCode();
                    var quest = response.Content.ReadAsAsync<QuestDTO>().Result;

                    model.QuestId = quest.Id;
                    return View(model);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
        }

        // POST: Design/Stage/Create
        [HttpPost]
        public async Task<ActionResult> Create(StageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["User"] as UserModel;

            model.Id = 0;

            var stage = mapper.Map<StageViewModel, StageDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.PostJsonAsync(@"api/Stage/Add", stage);
                    response.EnsureSuccessStatusCode();

                    return RedirectToAction("Details", "Quests", new { id = model.QuestId });
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return View(model);
                }
            }
        }

        [NotFoundException]
        // GET: Design/Stage/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.GetAsync(@"api/Stage/GetById?id=" + id);
                    response.EnsureSuccessStatusCode();

                    var stage = response.Content.ReadAsAsync<StageDTO>().Result;
                    var stageVM = mapper.Map<StageDTO, StageViewModel>(stage);

                    return View(stageVM);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
        }

        // POST: Design/Stage/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(StageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["User"] as UserModel;

            var stage = mapper.Map<StageViewModel, StageDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.PutJsonAsync(@"api/Stage/Update", stage);
                    response.EnsureSuccessStatusCode();

                    return RedirectToAction("Details", "Quests", new { id = model.QuestId });
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                    return View(model);
                }
            }

        }

        // POST: Design/Stage/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            var user = Session["User"] as UserModel;
            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.DeleteAsync(@"api/Stage/Delete?id=" + id);
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}
