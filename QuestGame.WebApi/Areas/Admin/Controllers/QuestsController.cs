using QuestGame.Common;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Infrastructura.CustomAutorize;
using QuestGame.WebApi.Models;
using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Admin.Controllers
{
    public class QuestsController : AuthController
    {
        #region вывод - Мои квесты
        // GET: Admin/Quests
        // [IsUserInfoInSession]
        public async Task<ActionResult> Index()
        {
            if (!this.IsAutherize()) { return RedirectToAction("Login", "Home", new { area = "" }); }

            ViewBag.Title = "Мои квесты";

            IEnumerable<QuestDTO> result;

            using (var client = new RequestApi())
            {
                result = await client.GetAsync<IEnumerable<QuestDTO>>(@"api/Quests/GetByUser?userIdentificator=" + GetUser().Identificator);
            }

            return View(result.OrderByDescending(o => o.AddDate));
        }
        #endregion

        #region Добавление Квеста
        public ActionResult AddQuest()
        {
            ViewBag.Title = "Добавление квеста";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddQuest(QuestVM model)
        {
            if (model.File != null)
            {
                string fileName = System.IO.Path.GetFileName(model.File.FileName);
                model.Image = @"/Content/Images/GameContent/" + fileName;
            }
            else
            {
                model.Image = "http://www.novelupdates.com/img/noimagefound.jpg";
            }

            using (var client = new RequestApi(GetUser().Token))
            {
                try
                {
                    var request = await client.PostJsonAsync(@"api/Quests/Add", model);
                    if (model.File != null)
                    {
                        string fileName = System.IO.Path.GetFileName(model.File.FileName);
                        model.File.SaveAs(Server.MapPath("~/Content/Images/GameContent/" + fileName));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Удание Квеста
        [HttpGet]
        public async Task<ActionResult> RemoveQuest(int id)
        {
            using (var client = new RequestApi(GetUser().Token))
            {
                var request = await client.DeleteAsync(@"api/Quests/Del?id=" + id);
            }

            return RedirectToAction("Index");
        }
        #endregion

        public async Task<ActionResult> EditQuest(int id)
        {
            QuestDTO quest;

            ViewBag.Title = "Редактирование квеста";

            using (var client = new RequestApi())
            {
                quest = await client.GetAsync<QuestDTO>(@"api/Quests/" + id);
            }

            return View(quest);
        }

        [HttpPost]
        public async Task<ActionResult> EditQuest(QuestVM model)
        {
            if (model.File != null)
            {
                string fileName = System.IO.Path.GetFileName(model.File.FileName);
                model.Image = @"/Content/Images/GameContent/" + fileName;
            }
            else
            {
                model.Image = "http://www.novelupdates.com/img/noimagefound.jpg";
            }

            using (var client = new RequestApi(GetUser().Token))
            {
                try
                {
                    var request = await client.PostJsonAsync(@"api/Quests/Edit", model);
                    if (model.File != null)
                    {
                        string fileName = System.IO.Path.GetFileName(model.File.FileName);
                        model.File.SaveAs(Server.MapPath("~/Content/Images/GameContent/" + fileName));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<ActionResult> AddStage(int id)
        //{
        //    var user = this.GetUser();
        //    IRequest client = new DirectRequest();
        //    var request = await client.GetRequestAsync(@"api/Quests");
        //    var response = await request.Content.ReadAsAsync<IEnumerable<Quest>>();

        //    var result = response.FirstOrDefault( q => q.Id == id );

        //    return View(result);
        //}

        //[HttpGet]
        //public async Task<ActionResult> QuestPreview(int id)
        //{
        //    IRequest client = new DirectRequest();
        //    var request = await client.GetRequestAsync(@"api/Quests");
        //    var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

        //    var result = response.FirstOrDefault(q => q.Id == id);

        //    return View(result);
        //}

        [HttpGet]
        public async Task<ActionResult> DesignQuest(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            IRequest client = new DirectRequest();
            var request = await client.GetRequestAsync(@"api/Quests");
            var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

            var result = response.FirstOrDefault(q => q.Id == id);

            return View(result);
        }
    }
}