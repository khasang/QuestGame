using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using QuestGame.WebApi.Attributes;
using QuestGame.WebApi.Models;
using System.IO;
using System.Web.Configuration;
using System.Net;
using QuestGame.Common.Helpers;

namespace QuestGame.WebApi.Controllers
{
    [CustomAuthorize]
    public class BaseController : Controller
    {
        protected IMapper mapper;

        public BaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected UserModel SessionUser
        {
            get
            {
                return Session["User"] as UserModel;
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        protected ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Temp/"), fileName);
                try
                {
                    file.SaveAs(path);         // сохраняем файл в папку Files в проекте
                }
                catch
                {
                    ViewBag.Message = "Ошибка локального сохранения";
                }

                RestHelper.UploadFile("api/UploadFile", path);

                System.IO.File.Delete(path);   // удаляем файл из папки Files в проекте

                ViewBag.Message = "Файл успешно отправлен";
            }
            else
            {
                ViewBag.Message = "Имя файла null";
            }
            return RedirectToAction("Index");
        }
    }
}