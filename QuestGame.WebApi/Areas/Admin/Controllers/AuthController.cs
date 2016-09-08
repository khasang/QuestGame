using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        UserProfileVM user = new UserProfileVM();


        protected bool IsAutherize()
        {
            return Session["UserInfo"] == null ? false : true;
        }

        protected UserProfileVM GetUser()
        {
            if (this.IsAutherize())
            {
                this.user = (UserProfileVM)Session["UserInfo"];
            }
            return user;
        }
    }
}