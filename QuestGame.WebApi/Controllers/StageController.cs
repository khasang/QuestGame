using AutoMapper;
using QuestGame.Common.Interfaces;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Stage")]
    public class StageController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public StageController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;

            logger.Information("Request | StageController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<StageDTO> GetAll()
        {
            var stages = dataManager.Stages.GetAll().ToList();

            var model = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stages);
            return model;
        }

        [HttpGet]
        [Route("GetById")]
        public StageDTO GetById(int id)
        {
            var stage = dataManager.Stages.GetById(id);

            var model = mapper.Map<Stage, StageDTO>(stage);
            return model;
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(StageDTO stage)
        {
            try
            {
                var model = mapper.Map<StageDTO, Stage>(stage);
                var owner = dataManager.Quests.GetById(stage.QuestId);
                if (owner == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                model.Quest = owner;

                dataManager.Stages.Add(model);
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
        public void Update(StageDTO stage)
        {
            var stageEntity = dataManager.Stages.GetById(stage.Id);
            var model = mapper.Map<StageDTO, Stage>(stage, stageEntity);

            var owner = dataManager.Quests.GetById(stage.QuestId);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.Quest = owner;
            dataManager.Stages.Update(model);
            dataManager.Save();
        }

        [HttpDelete]
        [Route("Delete")]
        public void Delete(StageDTO quest)
        {
            var model = mapper.Map<StageDTO, Stage>(quest);
            
            if (model != null)
            {
                dataManager.Stages.Delete(model.Id);
                dataManager.Save();
            }
        }

        [HttpGet]
        [Route("GetByQuestId")]
        public IEnumerable<StageDTO> GetByQuestId(int id)
        {
            var stages = dataManager.Stages.GetByQuestId(id);
            var model = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stages);

            return model;
        }
    }
}
