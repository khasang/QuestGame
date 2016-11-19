using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Constants
{
    public class ConfigSettings
    {
        #region Ключи параметров

        public const string WebApiServiceUrlKey = "WebApiServiceBaseUrl";

        #endregion

        #region Настройки по умолчанию

        public const string WebApiServiceBaseUrl = @"http://localhost:9243/";
        public const string RelativeFilePath = @"~/Content/Temp/";

        #endregion

    }
}