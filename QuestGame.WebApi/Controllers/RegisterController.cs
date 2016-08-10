using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;
using QuestGame.WebApi.Models;

namespace QuestGame.WebApi.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Страница регистрации";

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser( UserInvite user )
        {
            


            return View();
        }
    }
}
