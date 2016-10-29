using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Constants
{
    public class ConfigSettings
    {
        #region Ключи параметров

        public const string ServiceBaseUrlKey = "WebApiServiceBaseUrl";

        #endregion

        #region Настройки по умолчанию

        public const string WebApiServiceBaseUrl = @"http://localhost:9243/";

        #endregion

    }
}