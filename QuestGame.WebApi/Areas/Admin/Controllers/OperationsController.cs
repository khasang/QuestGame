using QuestGame.Domain.DTO;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult AddOperations(int? id)
        {
            if (!this.IsAutherize())
            {
                return RedirectToAction("Index", "Quests", new { area = "Admin" });
            }

            var modelPrepare = new OperationsVM(id);

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


    }
}
