using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Core;
using QuestGame.Common;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Quest")]
    public class QuestController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public QuestController(IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<QuestDTO> GetAll()
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorReadData));
            }
        }

        [HttpGet]
        [Route("GetByUser")]
        public IEnumerable<QuestDTO> GetByUser()
        {
            var principal = Thread.CurrentPrincipal;
            var identity = principal.Identity;
            var id = identity.GetUserId();

            try
            {
                var quests = dataManager.Quests.GetByUser(id.ToString()).ToList();
                var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorReadData));
            }
        }

        [HttpGet]
        [Route("GetById")]
        public QuestDTO GetById(int id)
        {
            try
            {
                var quest = dataManager.Quests.GetById(id);
                if (quest == null) { throw new ObjectNotFoundException(); }
                var response = mapper.Map<Quest, QuestDTO>(quest);
                return response;
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, MessageString.ErrorNotFound));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorReadData));
            }
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add([FromBody] QuestDTO quest)
        {
            if (quest == null)
            {
                return BadRequest();
            }

            var model = mapper.Map<QuestDTO, Quest>(quest);

            var principal = Thread.CurrentPrincipal;
            var identity = principal.Identity;
            var id = identity.GetUserId();

            try
            {
                model.OwnerId = id;
                model.Date = DateTime.Now;

                dataManager.Quests.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorSaveData));
            }
        }

        [HttpPut]
        [Route("Update")]
        public void Update([FromBody] QuestDTO model)
        {
            try
            {
                var questOriginal = dataManager.Quests.GetById(model.Id);
                if (questOriginal == null) { throw new ObjectNotFoundException(); }

                var questResult = mapper.Map<QuestDTO, Quest>(model, questOriginal);

                dataManager.Quests.Update(questResult);
                dataManager.Save();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorNotFound));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorSaveData));
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public void Delete(int id)
        {
            try
            {
                var quest = dataManager.Quests.GetById(id);
                if (quest == null) { throw new ObjectNotFoundException(); }

                dataManager.Quests.Delete(quest);
                dataManager.Save();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorNotFound));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorSaveData));
            }

        }
    }    
}

    
