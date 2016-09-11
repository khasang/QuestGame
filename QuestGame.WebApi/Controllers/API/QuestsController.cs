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
using Microsoft.AspNet.Identity;
using System.Net.Http;

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

        [HttpGet]
        [Route("GetByUser")]
        public IEnumerable<QuestDTO> GetQuestsByUser(string userIdentificator)
        {
            var quests = dataManager.Quests.GetByIdentificator(userIdentificator).ToList();
            var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);

            return response;
        }

        // GET: api/Quests/5
        [ResponseType(typeof(QuestDTO))]
        public IHttpActionResult GetQuest(int id)
        {
            var quest = dataManager.Quests.GetByID( id );
            if (quest == null)
            {
                return NotFound();
            }

            var response = mapper.Map<Quest, QuestDTO>(quest);

            return Ok(response); 
        }

        // DELETE
        [HttpDelete]
        [Route("Del")]
        public IHttpActionResult DeleteQuest(int id)
        {
            Quest quest = dataManager.Quests.GetByID(id);

            if (quest == null)
            {
                return NotFound();
            }

            dataManager.ContentQuest.Delete( quest.Content );

            dataManager.Quests.Delete( quest );
            dataManager.Save();

            return Ok();
        }

        // Add
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddQuest( QuestVM questVM )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quest = mapper.Map<QuestVM, Quest>(questVM);
            var content = mapper.Map<QuestVM, ContentQuest>(questVM);

            quest.Content = content;
            quest.AddDate = DateTime.Now;
            quest.CountComplite = 0;
            quest.Rate = 0;
            quest.ModifyDate = DateTime.Now;

            var principal = Thread.CurrentPrincipal;
            var identity = principal.Identity;
            var id = identity.GetUserId();

            if (id != null)
            {
                quest.UserId = id;
            }
            else
            {
                return BadRequest();
            }

            dataManager.Quests.Add(quest);
            dataManager.Save();

            return Ok(quest);
        }


        // Update
        [HttpPost]
        [Route("Edit")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Update(QuestVM questVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quest = mapper.Map<QuestVM, Quest>(questVM);
            var content = mapper.Map<QuestVM, ContentQuest>(questVM);

            quest.Content = content;
            quest.ModifyDate = DateTime.Now;

            var questEntity = dataManager.Quests.GetByID(quest.Id);

            questEntity.ModifyDate = DateTime.Now;
            //var contentEntity = questEntity.Content;
            //contentEntity.Image = content.Image;
            //contentEntity.Text = content.Text;
            content.ModifyDate = DateTime.Now;

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