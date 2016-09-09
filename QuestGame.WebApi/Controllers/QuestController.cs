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
    //[Authorize]
    [RoutePrefix("api/Quest")]
    public class QuestController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public QuestController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;

            logger.Information("Request | QuestController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<QuestDTO> GetAll()
        {
            var quests = dataManager.Quests.GetAll().ToList();

            var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);
            return response;
        }

        [HttpGet]
        [Route("GetById")]
        public QuestDTO GetById(int id)
        {
            var quest = dataManager.Quests.GetById(id);

            var response = mapper.Map<Quest, QuestDTO>(quest);
            return response;
        }

        [HttpGet]
        [Route("Details")]
        public /*IEnumerable<StageDTO>*/ /*IHttpActionResult*/int Details(string title)
        {
            //var quests = dataManager.Stages.GetAll().ToList();

            //var response = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(quests);
            return 10;
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(QuestFullDTO quest)
        {
            var model = mapper.Map<QuestFullDTO, Quest>(quest);

            var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.Owner = owner;
            model.Date = DateTime.Now;

            dataManager.Quests.Add(model);
            dataManager.Save();
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public void Update(QuestDTO quest)
        {
            var questEntity = dataManager.Quests.GetById(quest.Id);
            var model = mapper.Map<QuestDTO, Quest>(quest, questEntity);

            var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.Owner = owner;
            dataManager.Quests.Update(model);
            dataManager.Save();
        }

        //[HttpDelete]
        //[Route("Delete")]
        //public void Delete(QuestDTO quest)
        //{
        //    var model = mapper.Map<QuestDTO, Quest>(quest);

        //    // Пока не работает. Нужен Id
        //    //dataManager.Quests.Delete(model);
        //    //dataManager.Save();

        //    if (model != null && !string.IsNullOrEmpty(model.Title))
        //    {
        //        dataManager.Quests.DeleteByTitle(model.Title);
        //        dataManager.Save();
        //    }
        //}

        [HttpDelete]
        [Route("DelByTitle")]
        public void DelByTitle(string title)
        {
            //var model = mapper.Map<QuestFullDTO, Quest>(title);

            if (title != null && !string.IsNullOrEmpty(title))
            {
                dataManager.Quests.DeleteByTitle(title);
                dataManager.Save();
            }
        }
    }
}


