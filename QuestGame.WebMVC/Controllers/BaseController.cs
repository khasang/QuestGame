using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebMVC.Attributes;
using QuestGame.WebMVC.Constants;
using QuestGame.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        protected ApplicationUserDTO SessionUser
        {
            get
            {
                return Session["User"] as ApplicationUserDTO;
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        protected async Task<string> UploadFile(HttpPostedFileBase file)
        {
            if (file == null)
                throw new NullReferenceException(ErrorMessages.BaseErrorLoadFile);

            var fileName = Guid.NewGuid().ToString();    // Чтобы избежать возможного конфликта одинаковых имен
            var fileExt = Path.GetExtension(file.FileName);
            var path = Server.MapPath($"{DefaultParams.FileRelativePath}{fileName}{fileExt}");
            file.SaveAs(path);                              // сохраняем файл в папку Content/Temp в проекте

            string result;
            using (var client = RestHelper.Create(SessionUser.Token))
            using (var fileStream = System.IO.File.Open(path, FileMode.Open))
            {
                var fileInfo = new FileInfo(path);
                var content = new StreamContent(fileStream);
                var form = new MultipartFormDataContent();
                form.Add(content, DefaultParams.FileRelativePath, fileInfo.Name);

                var response = await client.PostAsync(ApiMethods.BaseUploadFile, form);
                result = await response.Content.ReadAsStringAsync();
            }

            //RestHelper.UploadFile(ApiMethods.BaseUploadFile, path); // Отправляем файл в слой WebApi
            System.IO.File.Delete(path);   // удаляем файл из папки Content/Temp в проекте

            return result;
        }
    }
}