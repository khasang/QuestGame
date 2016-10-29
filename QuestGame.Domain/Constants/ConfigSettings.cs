using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.Domain.Constants
{
    public class ConfigSettings
    {

        #region Ключи параметров

        public const string PathMailKey = "PathMail";

        #endregion

        #region Настройки по умолчанию

        public const string PathMail = @".\Content\Temp\";

        #endregion
    }
}