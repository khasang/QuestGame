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
            try
            {
                var stages = dataManager.Motions.GetAll().ToList();
                var model = mapper.Map<IEnumerable<Motion>, IEnumerable<MotionDTO>>(stages);
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | GetAll | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        [HttpGet]
        [Route("GetById")]
        public MotionDTO GetById(int? id)
        {
            try
            {
                var motion = dataManager.Motions.GetById(id);
                var model = mapper.Map<Motion, MotionDTO>(motion);
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | GetById | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("GetByStageId")]
        public IEnumerable<MotionDTO> GetByStageId(int? id)
        {
            try
            {
                var stage = dataManager.Motions.GetByStageId(id);
                var model = mapper.Map<IEnumerable<Motion>, IEnumerable<MotionDTO>>(stage);
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | GetByStageId | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(MotionDTO motion)
        {
            try
            {
                var model = mapper.Map<MotionDTO, Motion>(motion);
                var owner = dataManager.Stages.GetById(model.OwnerStageId);

                model.OwnerStage = owner;

                dataManager.Motions.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | Add | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(MotionDTO motion)
        {
            try
            {
                var model = mapper.Map<MotionDTO, Motion>(motion);
                var owner = dataManager.Stages.GetById(motion.OwnerStageId);

                model.OwnerStage = owner;

                dataManager.Motions.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | Create | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(MotionDTO motion)
        {
            try
            {
                var motionEntity = dataManager.Motions.GetById(motion.Id);
                var model = mapper.Map<MotionDTO, Motion>(motion, motionEntity);
                model.NextStageId = model.NextStageId == 0 ? null : model.NextStageId;

                dataManager.Motions.Update(model);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | Update | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(MotionDTO motion)
        {
            try
            {
                var model = mapper.Map<MotionDTO, Motion>(motion);
                dataManager.Motions.Delete(model.Id);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | Delete | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        [HttpDelete]
        [Route("DelById")]
        public IHttpActionResult DelById(int? id)
        {
            try
            {
                dataManager.Motions.Delete(id);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Motion | DelById | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
