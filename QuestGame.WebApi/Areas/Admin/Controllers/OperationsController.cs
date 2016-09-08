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
        IEnumerable<QuestDTO> listUserQuest = new List<QuestDTO>();

        public OperationsController() : base()
        {
            var user = this.GetUser();

            this.listUserQuest = UserQuestFill(user).Result;
        }


        // GET: Admin/Operations
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Quests", new { area = "Admin" });
        }

        // GET: Admin/Operations/Add/5
        public ActionResult AddOperations(int? id)
        {
            if (!this.IsAutherize())
            {
                return RedirectToAction("Index", "Quests", new { area = "Admin" });
            }

            var stages = listUserQuest.FirstOrDefault( q => q.Id == listUserQuest.SelectMany(x => x.Stages).FirstOrDefault(m => m.Id == id).QuestId).Stages;

            var modelPrepare = new OperationsVM_n(id);
            foreach (var item in stages)
            {
                modelPrepare.StagesList.Add(item.Id, item.Title);
            }

            return View(modelPrepare);
        }

        [HttpPost]
        public async Task<ActionResult> AddOperations(OperationsVM_n model)
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
