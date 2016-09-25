using AutoMapper;
using QuestGame.Common;
using QuestGame.Common.Interfaces;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Motion")]
    public class MotionController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public MotionController(IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetById")]
        public MotionDTO GetById(int id)
        {
            try
            {
                var motion = dataManager.Motions.GetById(id);
                if (motion == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, MessageString.ErrorNotFound));
                }
                var response = mapper.Map<Motion, MotionDTO>(motion);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorReadData));
            }
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add([FromBody] MotionDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var motion = mapper.Map<MotionDTO, Motion>(model);

            try
            {
                dataManager.Motions.Add(motion);
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
        public IHttpActionResult Update([FromBody] MotionDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            try
            {
                var motionOriginal = dataManager.Motions.GetById(model.Id);
                if (motionOriginal == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, MessageString.ErrorNotFound));
                }
                var motionResult = mapper.Map<MotionDTO, Motion>(model, motionOriginal);

                dataManager.Motions.Update(motionResult);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorSaveData));
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var motion = dataManager.Motions.GetById(id);
                if (motion == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, MessageString.ErrorNotFound));
                }

                dataManager.Motions.Delete(motion);
                dataManager.Save();

                return Ok(motion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MessageString.ErrorSaveData));
            }
        }
    }
}
