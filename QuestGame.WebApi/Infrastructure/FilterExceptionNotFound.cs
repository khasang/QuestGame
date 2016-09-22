using QuestGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Infrastructure
{
    public class NotFoundExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is HttpRequestException)
            {
                filterContext.Controller.TempData["Errors"] = filterContext.Exception.Message;
                filterContext.ExceptionHandled = true;

                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.UrlReferrer.AbsolutePath);
            }
        }
    }
}