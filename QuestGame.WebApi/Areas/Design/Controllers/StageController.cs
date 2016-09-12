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
    public class StageController : Controller
    {
        // GET: Design/Stage
        public ActionResult Index()
        {
            return View();
        }

        // GET: Design/Stage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Design/Stage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Design/Stage/Create
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

        // GET: Design/Stage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Design/Stage/Edit/5
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

        // POST: Design/Stage/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var user = Session["User"] as UserModel;

            using (var client = new RequestApi(user.Token))
            {
                var quest = await client.DeleteAsync(@"api/Stage/Delete?id=" + id);
            }

            return RedirectToAction("Index");
        }
    }
}
