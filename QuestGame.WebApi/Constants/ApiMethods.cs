using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Constants
{
    public class ApiMethods
    {
        #region AccountController

        public const string AccontUser = @"api/Account/GetUser?id=";
        public const string AccontUserLogin = @"api/Account/LoginUser";
        
        #endregion
    }
}