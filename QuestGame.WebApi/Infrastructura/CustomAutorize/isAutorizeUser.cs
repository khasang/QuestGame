using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Infrastructura.CustomAutorize
{
    public class isAutorizeUser : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserInfo"] == null ? false : true)
            {
               // RedirectToAction("Login", "Home", new { area = "" });
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Browser.Browser == "Opera")
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}