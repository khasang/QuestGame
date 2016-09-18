using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Infrastructure
{
    public class NotFoundExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
                    filterContext.Exception != null)
            {
                filterContext.HttpContext.Session["SomeErrors"] = " Ошибка из фильтра Ошибок";
                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.UrlReferrer.PathAndQuery);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}