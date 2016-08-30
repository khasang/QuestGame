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
using System.Threading;
using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Mappings;
using QuestGame.WebApi.Models;

namespace QuestGame.WebApi.Controllers
{
    [RoutePrefix("api/Quests")]
    public class QuestsController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public QuestsController( IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        // GET: api/Quests
        public IEnumerable<QuestDTO> GetQuests()
        {
            var quests = dataManager.Quests.GetAll().ToList();
            var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);

            return response;
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
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddQuest( QuestVM quest )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var q = mapper.Map<QuestVM, Quest>(quest);
            var c = mapper.Map<QuestVM, ContentQuest>(quest);

            q.Content = c;

            dataManager.Quests.Add(q);
            dataManager.Save();

            return Ok(q);
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