using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Constants
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

        #endregion

        #region MotionController

        public const string MotionGetByStageId = @"api/Motion/GetByStageId?id=";

        #endregion
    }
}