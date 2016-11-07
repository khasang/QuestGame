using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Constants
{
    public class DefaultParams
    {
        #region Ключи параметров

        public const string WebApiServiceUrlNameKey = "WebApiServiceBaseUrl";

        #endregion

        #region Настройки по умолчанию

        public const string WebApiServiceBaseUrl = @"http://localhost:9243/";
        public const string FileRelativePath = @"~/Content/Temp/";

        #endregion

    }
}