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
    public class RegisterMVCController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Страница регистрации";

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser( UserInvite user )
        {
            var result = new RegisterWebApiController().RegisterUser( user );
            
            ViewBag.Title = result;
            return View();
        }
    }
}
