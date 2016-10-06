using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Models
{
    public class FileModel
    {
        public int Id { get; set; }

        public string Ext { get; set; }

        public string GetFilePath()
        {
            return @"~/Contents/Images/" + GetFileName;
        }

        public string GetFileName
        {
            get { return string.Format("{0}.{1}", Id, Ext); }
        }

        public void DeleteFile(Controller controller)
        {
            File.Delete(controller.Server.MapPath(controller.Url.Content(@"~/Contents/Images/" + GetFileName)));
        }

        public static string GetExt(string fileName)
        {
            return fileName.Substring(fileName.Length - 3, 3);
        }
    }
}