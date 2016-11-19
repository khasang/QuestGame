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

        [CustomAuthorize]
        protected async Task<string> UploadFile(HttpPostedFileBase file, string apiUpload)
        {
            if (file == null)
                return string.Empty;

            var fileName = Guid.NewGuid().ToString();    // Чтобы избежать возможного конфликта одинаковых имен
            var fileExt = Path.GetExtension(file.FileName);
            var path = Server.MapPath($"{ConfigSettings.RelativeFilePath}{fileName}{fileExt}");
            file.SaveAs(path);                              // сохраняем файл в папку Content/Temp в проекте

            using (var client = RestHelper.Create(SessionUser.Token))
            using (var fileStream = System.IO.File.Open(path, FileMode.Open))
            {
                var fileInfo = new FileInfo(path);
                var content = new StreamContent(fileStream);
                var form = new MultipartFormDataContent();
                form.Add(content, ConfigSettings.RelativeFilePath, fileInfo.Name);

                var response = await client.PostAsync(apiUpload, form);
                var result = await response.Content.ReadAsAsync<string>();
                
                System.IO.File.Delete(path);   // удаляем файл из папки Content/Temp в проекте

                return result;
            }
        }
    }
}