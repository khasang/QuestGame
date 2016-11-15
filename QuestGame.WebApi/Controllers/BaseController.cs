using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using QuestGame.Common.Helpers;
using QuestGame.WebApi.Constants;

namespace QuestGame.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        protected async Task<string> Upload(string fileType)
        {
            var content = Request.Content;

            if (!content.IsMimeMultipartContent())
                throw new Exception();

            var path = ConfigSettings.GetAbsFilePath();
            var provider = new MultipartFormDataStreamProvider(path);

            var result = await content.ReadAsMultipartAsync(provider);
            
            if (provider.FileData.Count > 1)
                throw new FileLoadException(ErrorMessages.LoadOnlyOneFile);

            var file = new FileInfo(provider.FileData[0].LocalFileName);
            if (file.Length == 0)
                throw new Exception(ErrorMessages.DefectFile);

            var fileName = provider.FileData[0].Headers.ContentDisposition.FileName.Trim('"');
            var pathName = $"{path}{fileType}\\{fileName}";

            if (File.Exists(pathName))
                throw new Exception(ErrorMessages.ExistsFile);

            try
            {
                file.MoveTo(pathName); // Здесь мы можем настроить какие картинки где хранить
            }
            catch (Exception ex)
            {
                foreach (var fileData in provider.FileData)  // если какие-либо ошибки при перемещении,
                    File.Delete(fileData.LocalFileName);     // то удаляем загруженные файлы

                throw new Exception(ErrorMessages.DefectFile, ex);
            }

            var baseUrl = CommonHelper.GetConfigOrDefaultValue(ConfigSettings.BaseUrlKey, ConfigSettings.WebApiServiceBaseUrl);
            var urlFile = $"{baseUrl}Content/Images/{fileType}/{fileName}";
            return urlFile;
        }
    }
}