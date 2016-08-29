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
    [RoutePrefix("api/Motion")]
    public class MotionController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public MotionController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;

            logger.Information("Request | MotionController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
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
        public IHttpActionResult Add(MotionDTO motion)
        {
            try
            {
                var model = mapper.Map<MotionDTO, Motion>(motion);
                var owner = dataManager.Stages.GetById(model.OwnerStageId);
                if (owner == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                model.OwnerStage = owner;

                dataManager.Motions.Add(model);
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
        public void Update(MotionDTO motion)
        {
            var motionEntity = dataManager.Motions.GetById(motion.Id);
            var model = mapper.Map<MotionDTO, Motion>(motion, motionEntity);

            var owner = dataManager.Stages.GetById(motion.StageId);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.OwnerStage = owner;
            dataManager.Motions.Update(model);
            dataManager.Save();
        }

        [HttpDelete]
        [Route("Delete")]
        public void Delete(MotionDTO motion)
        {
            var model = mapper.Map<MotionDTO, Motion>(motion);

            if (model != null)
            {
                dataManager.Motions.Delete(model.Id);
                dataManager.Save();
            }
        }
    }
}
