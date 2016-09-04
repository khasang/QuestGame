using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Controllers.MVC
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        public ActionResult Index(UserRegisterVM user)
        {
            ViewBag.Title = "Профиль пользователя - " + user.Name + " " + user.LastName;
            return View(user);
        }
    }
}