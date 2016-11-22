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

            var provider = new MultipartMemoryStreamProvider();
            var result = await content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count != 1)
                throw new FileLoadException(ErrorMessages.LoadOnlyOneFile);

            var stream = provider.Contents[0];
            var fileName = Guid.NewGuid().ToString();    // Чтобы избежать возможного конфликта одинаковых имен
            var fileExt = Path.GetExtension(stream.Headers.ContentDisposition.FileName.Trim('"'));
            
            var imgStream = await stream.ReadAsStreamAsync();
            var img = GetImageResize(Image.FromStream(imgStream), 200, 200); // константы перенести в конфиг

            img.Save($"{path}{prefix}\\{fileName}{fileExt}");
            
            var baseUrl = CommonHelper.GetConfigOrDefaultValue(ConfigSettings.BaseUrlKey, ConfigSettings.WebApiServiceBaseUrl);
            var urlFile = $"{baseUrl}Content/Images/{prefix}/{fileName}{fileExt}";
            return urlFile;
        }

        private Image GetImageResize(Image img, int width, int height)
        {
            int newWidth;
            int newHeight;

            var k = (double)img.Width / (double)img.Height;

            if (k > 1)
            {
                newWidth = img.Width > width ? width : img.Width;
                newHeight = (int)(newWidth / k);
            }
            else
            {
                newHeight = img.Height > height ? height : img.Height;
                newWidth = (int)(newHeight * k);
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);

            //int left = newWidth < width ? (width - newWidth) / 2 : 0;
            //int top = newHeight < height ? (height - newHeight) / 2 : 0;

            thumbnailGraph.DrawImage(img, imageRectangle);

            return thumbnailBitmap;
        }
    }
}