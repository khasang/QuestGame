using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace QuestGame.WebMVC.Attributes
{
    public class HTTPExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception != null)
            {
                filterContext.Controller.ViewBag.Title = "Ошибка";
                filterContext.Controller.ViewBag.Message = filterContext.Exception.Message;

                filterContext.ExceptionHandled = true;

                filterContext.Result = new ViewResult
                {
                    ViewName = "ActionResultInfo",
                    ViewData = filterContext.Controller.ViewData,
                    TempData = filterContext.Controller.TempData
                };
            }
        }
    }
}

