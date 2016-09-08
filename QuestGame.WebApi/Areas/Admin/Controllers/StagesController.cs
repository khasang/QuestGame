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

            var modelPrepare = new StageVM
            {
                QuestId = id,
                Title = "Сцена - "
            };

            return View(modelPrepare);
        }

        [HttpPost]
        public async Task<ActionResult> AddStage(StageVM model)
        {
            var user = GetUser();
            var client = new AuthRequest(user.Token);
            var response = await client.PostRequestAsync(@"api/Stages/Add", model);

            return RedirectToAction("EditQuest", "Quests", new { id = model.QuestId });
        }

        [HttpGet]
        public async Task<ActionResult> EditStage(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            IRequest client = new DirectRequest();
            var request = await client.GetRequestAsync(@"api/Stages");
            var response = await request.Content.ReadAsAsync<IEnumerable<StageDTO>>();

            var result = response.FirstOrDefault(s => s.Id == id);

            return View(result);
        }
    }
}