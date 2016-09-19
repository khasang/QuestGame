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
using System.Data.Entity.Core;

namespace QuestGame.WebApi.Controllers
{
    [RoutePrefix("api/Quests")]
    public class QuestsController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public QuestsController(IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        // GET: api/Quests
        public IEnumerable<QuestDTO> GetQuests()
        {
            try
            {
                var quests = dataManager.Quests.GetAll().ToList();
                var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка получения данных")));
            }
        }


        [HttpGet]
        [Route("GetById")]
        public QuestDTO GetById(int id)
        {
            try
            {
                var quest = dataManager.Quests.GetByID(id);
                if (quest == null) { throw new ObjectNotFoundException(); }
                var response = mapper.Map<Quest, QuestDTO>(quest);
                return response;
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Элемент не найден")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка получения данных")));
            }
        }


        [HttpGet]
        [Route("GetByUser")]
        public IEnumerable<QuestDTO> GetQuestsByUser(string userIdentificator)
        {
            try
            {
                var quests = dataManager.Quests.GetByIdentificator(userIdentificator).ToList();
                var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка получения данных")));
            }
        }

        // Add
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddQuest(QuestVM questVM)
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

            try
            {
                dataManager.Quests.Add(quest);
                dataManager.Save();

                return Ok(quest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка сохранения данных")));
            }

        }

        // Update
        [HttpPost]
        [Route("Edit")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Update(QuestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questEntity = dataManager.Quests.GetByID(model.Id);
            if (questEntity == null) { throw new ObjectNotFoundException(); }
            var questResult = mapper.Map<QuestDTO, Quest>(model, questEntity);

            questResult.ModifyDate = DateTime.Now;

            dataManager.Quests.Update(questResult);

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

            dataManager.ContentQuest.Delete(quest.Content);

            dataManager.Quests.Delete(quest);
            dataManager.Save();

            return Ok();
        }








    }
}