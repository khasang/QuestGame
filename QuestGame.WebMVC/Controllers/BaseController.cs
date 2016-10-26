using AutoMapper;
using QuestGame.WebMVC.Attributes;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Helpers;
using QuestGame.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Controllers
{
    [CustomAuthorize]
    public class BaseController : Controller
    {
        protected IMapper mapper;

        public BaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected string WebApiServiceUrl
        {
            get
            {
                return WebConfigurationManager.AppSettings[DefaultParams.WebApiServiceUrlNameKey];
            }
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

        protected string UploadFile(HttpPostedFileBase file)
        {
            string errorMessage = string.Empty;
            string path = string.Empty;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();    // Чтобы избежать возможного конфликта одинаковых имен
                string fileExt = Path.GetExtension(file.FileName);
                path = Server.MapPath($"{DefaultParams.ImageRelativePath}{fileName}{fileExt}");
                file.SaveAs(path);                              // сохраняем файл в папку Content/Temp в проекте

                using (var client = RestHelper.Create(SessionUser.Token))
                using(var fileStream = System.IO.File.Open(path, FileMode.Open))
                {
                    var fileInfo = new FileInfo(path);
                    var content = new StreamContent(fileStream);
                    var form = new MultipartFormDataContent();
                    form.Add(content, DefaultParams.ImageRelativePath, fileInfo.Name);
                    //form.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var response = client.PostAsync(ApiMethods.BaseUploadFile, form).Result;
                }







                RestHelper.UploadFile(ApiMethods.BaseUploadFile, Server.MapPath(path)); // Отправляем файл в слой WebApi
                System.IO.File.Delete(path);   // удаляем файл из папки Content/Temp в проекте
            }

            return path;
        }
    }
}