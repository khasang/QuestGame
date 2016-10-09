using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Constants
{
    public class ApiMethods
    {
        #region QuestController

        public const string QuestGetById = @"api/Quest/GetById?id=";
        public const string QuestGetAll = @"api/Quest/GetAll";
        public const string QuestGetByActive = @"api/Quest/GetByActive";
        public const string QuestGetUserName = @"api/Quest/GetByUserName?userName=";
        public const string QuestUpdate = @"api/Quest/Update";

        #endregion

        #region QuestFullController

        public const string QuestFullCreate = @"api/QuestFull/Create";
        public const string QuestFullDelByTitle = @"api/QuestFull/DelByTitle?title=";
        public const string QuestFullDelById = @"api/QuestFull/DelById?id=";
        public const string QuestFullGetById = @"api/QuestFull/GetById?id=";

        #endregion

        #region StageController

        public const string StageGetByQuestId = @"api/Stage/GetByQuestId?id=";
        public const string StageGetById = @"api/Stage/GetById?id=";
        public const string StageCreate = @"api/Stage/Create";
        public const string StageUpdate = @"api/Stage/Update";
        public const string StageDelById = @"api/Stage/DelById?id=";
        #endregion

        #region MotionController

        public const string MotionGetByStageId = @"api/Motion/GetByStageId?id=";
        public const string MotionGetById = @"api/Motion/GetById?id=";
        public const string MotionCreate = @"api/Motion/Create";
        public const string MotionUpdate = @"api/Motion/Update";
        public const string MotionDelById = @"api/Motion/DelById?id=";

        #endregion

        #region AccountController

        public const string AccountLogin = @"api/Account/LoginUser";
        public const string AccontRegister = @"api/Account/Register";
        public const string AccontUser = @"api/Account/GetUser?id=";
        public const string AccontEditUser = @"api/Account/EditUser";

        #endregion

        public const string BaseUploadFile = @"api/UploadFile";
    }
}