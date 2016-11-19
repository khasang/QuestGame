using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using QuestGame.Common.Helpers;
using QuestGame.WebApi.Constants;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QuestGame.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        protected async Task<string> Upload(string prefix)
        {
            var content = Request.Content;

            if (!content.IsMimeMultipartContent())
                throw new Exception();

            var path = ConfigSettings.GetLocalFilePath();
            var provider = new MultipartFormDataStreamProvider(path);

            if (provider.FileData.Count > 1)
                throw new FileLoadException(ErrorMessages.LoadOnlyOneFile);

            var result = await content.ReadAsMultipartAsync(provider);

            var file = new FileInfo(provider.FileData[0].LocalFileName);
            if (file.Length == 0)
                throw new Exception(ErrorMessages.DefectFile);

            var fileName = provider.FileData[0].Headers.ContentDisposition.FileName.Trim('"');
            var filePath = $"{path}{prefix}\\{fileName}";

            if (File.Exists(filePath))
                throw new Exception(ErrorMessages.ExistsFile);

            try
            {
                file.MoveTo(filePath); // Здесь мы можем настроить какие картинки где хранить

                //var newImg = ImageResize(img, 600, 800);
                //newImg.Save(filePath);
            }
            catch (Exception ex)
            {
                foreach (var fileData in provider.FileData)  // если какие-либо ошибки при перемещении,
                    File.Delete(fileData.LocalFileName);     // то удаляем загруженные файлы

                throw new Exception(ErrorMessages.DefectFile, ex);
            }

            var baseUrl = CommonHelper.GetConfigOrDefaultValue(ConfigSettings.BaseUrlKey, ConfigSettings.WebApiServiceBaseUrl);
            var urlFile = $"{baseUrl}Content/Images/{prefix}/{fileName}";
            return urlFile;
        }

        //private Image ImageResize(Image img, int height, int width)
        //{
        //    var stream = new MemoryStream()
        //    var img = Image.FromStream()
        //    var bmp = new Bitmap(img, 600, 800);

        //    using (Graphics g = Graphics.FromImage((Image)bmp))
        //    {
        //        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        g.DrawImage(img, 0, 0, width, height);
        //    }

        //    return bmp;
        //}
    }
}