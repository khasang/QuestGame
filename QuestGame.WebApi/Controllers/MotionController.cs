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
        public IEnumerable<MotionDTO> GetAll()
        {
            var stages = dataManager.Motions.GetAll().ToList();

            var model = mapper.Map<IEnumerable<Motion>, IEnumerable<MotionDTO>>(stages);
            return model;
        }

        [HttpGet]
        [Route("GetById")]
        public MotionDTO GetById(int id)
        {
            var stage = dataManager.Motions.GetById(id);

            var model = mapper.Map<Motion, MotionDTO>(stage);
            return model;
        }

        [HttpGet]
        [Route("GetByStageId")]
        public IEnumerable<MotionDTO> GetByStageId(int id)
        {
            var stage = dataManager.Motions.GetByStageId(id);

            var model = mapper.Map<IEnumerable<Motion>, IEnumerable<MotionDTO>>(stage);
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
        public IHttpActionResult Update(MotionDTO motion)
        {
            var motionEntity = dataManager.Motions.GetById(motion.Id);
            var model = mapper.Map<MotionDTO, Motion>(motion, motionEntity);

            var owner = dataManager.Stages.GetById(motion.StageId);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.OwnerStage = owner;
            dataManager.Motions.Update(model);
            dataManager.Save();
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(MotionDTO motion)
        {
            var model = mapper.Map<MotionDTO, Motion>(motion);

            if (model == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            dataManager.Motions.Delete(model.Id);
            dataManager.Save();
            return Ok();
        }
    }
}
