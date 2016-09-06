using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Admin.Controllers
{
    public class UserProfileController : Controller
    {
        UserProfileVM user = new UserProfileVM();

        // GET: UserProfile
        public ActionResult Index()
        {
            if (!this.IsAutherize())
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            user = this.GetUser();

            ViewBag.Title = "Профиль пользователя";
            return View(user);
        }
        //// GET: UserProfile
        //public ActionResult Index(UserRegisterVM user)
        //{
        //    ViewBag.Title = "Профиль пользователя - " + user.Name + " " + user.LastName;
        //    return View(user);
        //}

        private bool IsAutherize()
        {
            return Session["UserInfo"] == null ? false : true;
        }

        private UserProfileVM GetUser()
        {
            if (this.IsAutherize())
            {
                this.user = (UserProfileVM)Session["UserInfo"];
            }
            return user;
        }
    }
}