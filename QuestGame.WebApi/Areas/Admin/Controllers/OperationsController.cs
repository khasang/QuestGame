using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
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
    public class OperationsController : AuthController
    {
        // GET: Admin/Operations
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Quests", new { area = "Admin" });
        }

        // GET: Admin/Operations/Add/5
        public async Task<ActionResult> AddOperations(int? id)
        {
            if (!this.IsAutherize())
            {
                return RedirectToAction("Index", "Quests", new { area = "Admin" });
            }

            var user = this.GetUser();

            //var listUserQuest = UserQuestFill(user).Result;

            IRequest client = new DirectRequest();
            var request = await client.GetRequestAsync(@"api/Quests");
            var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

            var stages = response.FirstOrDefault( q => q.Id == response.SelectMany(x => x.Stages).FirstOrDefault(m => m.Id == id).QuestId).Stages;

            var modelPrepare = new OperationsVM();

            modelPrepare.StageId = (int)id;

            foreach (var item in stages)
            {
                modelPrepare.StagesList.Add(item.Id, item.Title);
            }

            return View(modelPrepare);
        }

        [HttpPost]
        public async Task<ActionResult> AddOperations(OperationsVM model)
        {
            var user = GetUser();
            var client = new AuthRequest(user.Token);
            var response = await client.PostRequestAsync(@"api/Operations/Add", model);

            return RedirectToAction("EditStage", "Stages", new { id = model.StageId });
        }


        private async Task<IEnumerable<QuestDTO>> UserQuestFill(UserProfileVM user)
        {
            IEnumerable<QuestDTO> result = new List<QuestDTO>();

            IRequest client = new DirectRequest();
            var request = await client.GetRequestAsync(@"api/Quests");
            var response = await request.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

            result = from quest in response where quest.UserId == user.Id select quest;

            return result;
        }

    }
}
