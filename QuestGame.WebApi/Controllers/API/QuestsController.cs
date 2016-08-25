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

        public QuestsController( IDataManager dataManager )
        {
            this.dataManager = dataManager;
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

        // DELETE
        [ResponseType(typeof(Quest))]
        public IHttpActionResult DeleteQuest(int id)
        {
            Quest quest = dataManager.Quests.GetByID(id);

            if (quest == null)
            {
                return NotFound();
            }

            dataManager.Quests.Delete( quest );
            dataManager.Save();

            return Ok( quest );
        }

        // Add
        [ResponseType(typeof(Quest))]
        public IHttpActionResult AddQuest(Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dataManager.Quests.Add(quest);
            dataManager.Save();

            return CreatedAtRoute("DefaultApi", new { id = quest.Id }, quest);
        }

        // Update
        [ResponseType(typeof(void))]
        public IHttpActionResult Update(int id, Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quest.Id)
            {
                return BadRequest();
            }

            dataManager.Quests.Update(quest);

            try
            {
                dataManager.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}