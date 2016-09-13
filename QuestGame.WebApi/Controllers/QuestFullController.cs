using AutoMapper;
using Newtonsoft.Json;
using QuestGame.Domain;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using QuestGame.Common.Interfaces;
using System.Web;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/QuestFull")]
    public class QuestFullController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public QuestFullController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;

            //logger.Information("Request | QuestController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<QuestFullDTO> GetAll()
        {
            var quests = dataManager.Quests.GetAll().ToList();

            var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestFullDTO>>(quests);
            return response;            
        }

        [HttpGet]
        [Route("GetById")]
        public QuestFullDTO GetById(int id)
        {
            var quest = dataManager.Quests.GetById(id);

            var response = mapper.Map<Quest, QuestFullDTO>(quest);
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(QuestFullDTO quest)
        {
            var model = mapper.Map<QuestFullDTO, Quest>(quest);

            try
            {
                //var owner = dataManager.UserManager.FindByNameAsync(quest.Owner);
                var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
                if (owner == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                model.Owner = owner;
                model.Date = DateTime.Now;

                dataManager.Quests.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("Update")]
        public void Update(QuestFullDTO quest)
        {
            var questEntity = dataManager.Quests.GetById(quest.Id);
            mapper.Map<QuestFullDTO, Quest>(quest, questEntity);

            var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            dataManager.Quests.Update(questEntity);
            dataManager.Save();
        }

        [HttpDelete]
        [Route("Delete")]
        public void Delete(QuestFullDTO quest)
        {
            var model = mapper.Map<QuestFullDTO, Quest>(quest);

            // Пока не работает. Нужен Id
            //dataManager.Quests.Delete(model);

            if (model != null && !string.IsNullOrEmpty(model.Title))
            {
                dataManager.Quests.DeleteByTitle(model.Title);
                dataManager.Save();
            }
        }

        [HttpDelete]
        [Route("DelByTitle")]
        public IHttpActionResult DelByTitle(string title)
        {
            try
            {
                dataManager.Quests.DeleteByTitle(title);
                dataManager.Save();
                return Ok();
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }    
}

    
