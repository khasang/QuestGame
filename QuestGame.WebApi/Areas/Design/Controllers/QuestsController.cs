using QuestGame.Common;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class QuestsController : Controller
    {
        // GET: Design/Quests
        public async Task<ActionResult> Index()
        {
            IEnumerable<QuestDTO> quests = new List<QuestDTO>();

            ViewBag.Title = "Список доступных квестов";

            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token)) 
            {
                quests = await client.GetAsync<IEnumerable<QuestDTO>>(@"api/Quest/GetAll");
                return View(quests);
            }
        }

        // GET: Design/Quests/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Design/Quests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Design/Quests/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Design/Quests/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Design/Quests/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Design/Quests/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Design/Quests/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
