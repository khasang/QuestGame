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
    public class StageController : Controller
    {
        ICollection<string> ErrorsMessage = new List<string>();
        ICollection<string> InfoMessage = new List<string>();
        ICollection<string> WarningMessage = new List<string>();

        IMapper mapper;

        public StageController(IMapper mapper)
        {
            this.mapper = mapper;
        }


        [NotFoundException]
        // GET: Design/Stage/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.Title = "Details";

            var user = Session["User"] as UserModel;

            if (id == null)
            {
                ErrorsMessage.Add("Неправильный запрос");
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

            using (var client = new RequestApi(user.Token))
            {
                //try
                //{
                    var response = await client.GetAsync(@"api/Stage/GetById?id=" + 200);
                    response.EnsureSuccessStatusCode();

                    var stage = response.Content.ReadAsAsync<StageDTO>().Result;
                    if (stage == null)
                    {
                        ErrorsMessage.Add("ТАкого квеста не существует");

                        throw new Exception("Нет такого");

                        //return Redirect(Request.UrlReferrer.PathAndQuery);
                    }

                    var stageVM = mapper.Map<StageDTO, StageViewModel>(stage);

                    return View(stageVM);
                //}
                //catch (Exception ex)
                //{
                //    ErrorsMessage.Add("Неправильный запрос");

                //    var r = Session["ErrorException"];
                //    return Redirect(Request.UrlReferrer.PathAndQuery);
                //}
            }
        }

        // GET: Design/Stage/Create
        public async Task<ActionResult> Create(int? id) // Приходит id квеста владельца
        {
            var user = Session["User"] as UserModel;

            if (id == null)
            {
                ErrorsMessage.Add("Неправильный запрос");
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

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
                catch (Exception ex)
                {
                    ErrorsMessage.Add("Невозможно получить владельца. Ошибка сервера.");
                    ErrorsMessage.Add(ex.Message);

                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
        }

        // POST: Design/Stage/Create
        [HttpPost]
        public async Task<ActionResult> Create(StageViewModel model)
        {
            var user = Session["User"] as UserModel;

            if (!ModelState.IsValid)
            {
                ErrorsMessage.Add("Данные введены не верно либо не полностью.");
                return View(model);
            }

            model.Id = 0;

            var stage = mapper.Map<StageViewModel, StageDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.PostJsonAsync(@"api/Stage/Add", stage);
                    return RedirectToAction("Details", "Quests", new { id = model.QuestId });
                }
                catch (Exception ex)
                {
                    ErrorsMessage.Add("Неправильный запрос");
                    ErrorsMessage.Add(ex.Message);
                    return View(model);
                }
            }
        }

        // GET: Design/Stage/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var user = Session["User"] as UserModel;

            if (id == null)
            {
                ErrorsMessage.Add("Неправильный запрос");
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

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
                catch (Exception ex)
                {
                    ErrorsMessage.Add("Сцена не обновлена. Ошибка сервера.");
                    ErrorsMessage.Add(ex.Message);

                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }

            }
        }

        // POST: Design/Stage/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(StageViewModel model)
        {
            var user = Session["User"] as UserModel;

            if (!ModelState.IsValid)
            {
                ErrorsMessage.Add("Данные введены не верно либо не полностью.");
                return View(model);
            }

            var stage = mapper.Map<StageViewModel, StageDTO>(model);

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.PutJsonAsync(@"api/Stage/Update", stage);
                    response.EnsureSuccessStatusCode();
                    InfoMessage.Add("Сцена успешно обновлена!");

                    return RedirectToAction("Details", "Quests", new { id = model.QuestId });
                }
                catch (Exception ex)
                {
                    ErrorsMessage.Add("Сцена не обновлена. Ошибка сервера.");
                    ErrorsMessage.Add(ex.Message);

                    return View(model);
                }
            }

        }

        // POST: Design/Stage/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            var user = Session["User"] as UserModel;

            if (id == null)
            {
                ErrorsMessage.Add("Неправильный запрос");
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

            using (var client = new RequestApi(user.Token))
            {
                try
                {
                    var response = await client.DeleteAsync(@"api/Stage/Delete?id=" + id);
                    response.EnsureSuccessStatusCode();
                    InfoMessage.Add("Сцена успешно удалена!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ErrorsMessage.Add("Сцена не удалена.");
                    ErrorsMessage.Add(ex.Message);
                }
            }

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}
