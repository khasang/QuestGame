using AutoMapper;
using QuestGame.Common.Interfaces;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Infrastructure.Messages;
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
    [RoutePrefix("api/Stage")]
    public class StageController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public StageController(IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        // GET: api/Stages
        public IEnumerable<StageDTO> GetAll()
        {
            try
            {
                var stages = dataManager.Stages.GetAll().ToList();
                var response = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stages);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorReadData));
            }
        }

        [HttpGet]
        [Route("GetById")]
        public StageDTO GetById(int id)
        {
            try
            {
                var stage = dataManager.Stages.GetById(id);
                if (stage == null) { throw new ObjectNotFoundException(); }
                var response = mapper.Map<Stage, StageDTO>(stage);
                return response;
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, WebApiMessages.ErrorNotFound));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorReadData));
            }
        }

        // Create
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddStage([FromBody] StageDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            try
            {
                var stage = mapper.Map<StageDTO, Stage>(model);

                dataManager.Stages.Add(stage);
                dataManager.Save();

                return Ok(stage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorSaveData));
            }
        }

        // Update
        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] StageDTO model)
        {
            try
            {
                var stageOriginal = dataManager.Stages.GetById(model.Id);
                if (stageOriginal == null) { throw new ObjectNotFoundException(); }

                var stageResult = mapper.Map<StageDTO, Stage>(model, stageOriginal);

                dataManager.Stages.Update(stageResult);
                dataManager.Save();

                return Ok();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorNotFound));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorSaveData));
            }
        }

        // Delete
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var stage = dataManager.Stages.GetById(id);
                if (stage == null) { throw new ObjectNotFoundException(); }

                dataManager.Stages.Delete(stage);
                dataManager.Save();
                return Ok();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorNotFound));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WebApiMessages.ErrorSaveData));
            }
        }

    }
}
