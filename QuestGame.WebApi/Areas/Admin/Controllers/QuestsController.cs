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
        //UserProfileVM user  = new UserProfileVM();

        //IEnumerable<QuestDTO> userQuests = new List<QuestDTO>();

        public QuestsController()
        {
            //this.userQuests = GetAllUserQuests().Result;
        }

        // GET: Admin/Quests
        // [IsUserInfoInSession]
        public async Task<ActionResult> Index()
        {
            if (!this.IsAutherize()) { return RedirectToAction("Login", "Home", new { area = "" }); }

            ViewBag.Title = "Мои квесты";

            IEnumerable<QuestDTO> result;

            using (var client = new RequestApi())
            {
                var request = await client.GetAsync(@"api/Quests");
                var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();
                result = from quest in response where quest.UserId == GetUser().Id select quest;
            }
            
            return View(result.OrderByDescending(o => o.AddDate));
        }

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
                model.File.SaveAs(Server.MapPath("~/Content/Images/GameContent/" + fileName));
                model.Image = @"/Content/Images/GameContent/" + fileName;
            }
            else
            {
                model.Image = "http://www.novelupdates.com/img/noimagefound.jpg";
            }

            using (var client = new RequestApi(GetUser().Token))
            {
                var request = await client.PostJsonAsync(@"api/Quests/Add", model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> RemoveQuest(int id)
        {
            var client = new DirectRequest();
            var response = await client.DeleteRequestAsync(@"api/Quests/Del?id=" + id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> AddStage(int id)
        {
            var user = this.GetUser();
            IRequest client = new DirectRequest();
            var request = await client.GetRequestAsync(@"api/Quests");
            var response = await request.Content.ReadAsAsync<IEnumerable<Quest>>();

            var result = response.FirstOrDefault( q => q.Id == id );

            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> QuestPreview(int id)
        {
            IRequest client = new DirectRequest();
            var request = await client.GetRequestAsync(@"api/Quests");
            var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

            var result = response.FirstOrDefault(q => q.Id == id);

            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> EditQuest(int? id)
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

        //private async Task<IEnumerable<QuestDTO>> GetAllUserQuests()
        //{
        //    this.user = this.GetUser();
        //    IRequest client = new DirectRequest();
        //    var request = await client.GetRequestAsync(@"api/Quests");
        //    var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

        //    var result = from quest in response where quest.UserId == user.Id select quest;

        //    return result;
        //}

        //private bool IsAutherize()
        //{
        //    return Session["UserInfo"] == null ? false : true;
        //}

        //private UserProfileVM GetUser()
        //{
        //    if (this.IsAutherize())
        //    {
        //        this.user = (UserProfileVM)Session["UserInfo"];
        //    }
        //    return user;
        //}

    }
}