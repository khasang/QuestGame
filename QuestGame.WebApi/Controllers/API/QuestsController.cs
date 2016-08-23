using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using QuestGame.Domain;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Implementations;
using System;
using QuestGame.Domain.Interfaces;

namespace QuestGame.WebApi.Controllers
{
    public class QuestsController : ApiController
    {
        IDataManager dataManager;

        public QuestsController()
        {
            this.dataManager = new EFDataManager();
        }

        // GET: api/Quests
        public IEnumerable<Quest> GetQuests()
        {
            return dataManager.Quests.GetAll().ToList();
        }

        // GET: api/Quests/5
        [ResponseType(typeof(Quest))]
        public IHttpActionResult GetQuest(int id)
        {
            var quest = dataManager.Quests.GetByID( id );
            if (quest == null)
            {
                return NotFound();
            }

            return Ok(quest);
        }
    }
}