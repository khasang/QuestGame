using Newtonsoft.Json;
using QuestGame.Domain;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuestGame.WebApi.Controllers
{
    [RoutePrefix("api/Quest")]
    public class QuestController : ApiController
    {
        DataManager dataManager;

        public QuestController()
        {
            this.dataManager = new DataManager();
        }

        [Route("Get")]
        public string GetQuest()
        {
            List<Quest> quests = dataManager.Quests.GetAll().ToList();

            var response = JsonConvert.SerializeObject(quests);

            return response;
        }

    }
}
