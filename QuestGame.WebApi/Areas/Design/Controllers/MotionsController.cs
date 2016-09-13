using QuestGame.Common;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class MotionsController : Controller
    {
        // GET: Design/Motions
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Quests");
        }

        // GET: Design/Motions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Design/Motions/Create
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

        // GET: Design/Motions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Design/Motions/Edit/5
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

        // GET: Design/Motions/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quest = await client.DeleteAsync(@"api/Motion/Delete?id=" + id);
            }

            return RedirectToAction("Index", "Quests");
        }
    }
}
