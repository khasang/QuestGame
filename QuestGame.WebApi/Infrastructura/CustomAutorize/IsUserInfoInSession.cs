using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace QuestGame.WebApi.Infrastructura.CustomAutorize
{
    public class IsUserInfoInSession : AuthorizeAttribute
    {
        public IsUserInfoInSession()
        { }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return Session["UserInfo"] == null ? false : true;
            return false;
        }
    }
}