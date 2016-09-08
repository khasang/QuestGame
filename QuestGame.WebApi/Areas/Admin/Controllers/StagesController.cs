using QuestGame.Domain.DTO;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using QuestGame.WebApi.Models;

namespace QuestGame.WebApi.Areas.Admin.Controllers
{
    public class StagesController : AuthController
    {
        //GET: Admin/Stages
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AddStage(int id)
        {
            if (!this.IsAutherize())
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            var stagePrepare = new StageVM();
            stagePrepare.QuestId = id;
            stagePrepare.Title = "Сцена - ";

            return View(stagePrepare);
        }

        [HttpPost]
        public async Task<ActionResult> AddStage(StageVM model)
        {
            var user = GetUser();
            var client = new AuthRequest(user.Token);
            var response = await client.PostRequestAsync(@"api/Stages/Add", model);

            return RedirectToAction("EditQuest", "Quests", new { id = model.QuestId });
        }
    }
}