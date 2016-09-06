using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected UserProfileVM CurrentAuthUser;

        public BaseController() : base()
        {
            CurrentAuthUser = new UserProfileVM();
            CurrentAuthUser = this.GetUser();
        }

        protected bool IsAutherize()
        {
            return CurrentAuthUser == null ? false : true;
        }

        private UserProfileVM GetUser()
        {
            if (this.IsAutherize())
            {
                this.CurrentAuthUser = (UserProfileVM)Session["UserInfo"];
            }
            return CurrentAuthUser;
        }
    }
}