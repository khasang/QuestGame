using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Admin.Controllers
{
    public class UserProfileController : AuthController
    {
        // GET: UserProfile
        public ActionResult Index()
        {
            if (!this.IsAutherize())
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            var user = this.GetUser();

            ViewBag.Title = "Профиль пользователя";
            return View(user);
        }
        //// GET: UserProfile
        //public ActionResult Index(UserRegisterVM user)
        //{
        //    ViewBag.Title = "Профиль пользователя - " + user.Name + " " + user.LastName;
        //    return View(user);
        //}

    }
}