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
    [RoutePrefix("api/Stages")]
    public class StagesController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public StagesController( IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        // GET: api/Stages
        public IEnumerable<StageDTO> GetStages()
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка получения данных")));
            }           
        }

        [HttpGet]
        [Route("GetById")]
        public StageDTO GetById(int id)
        {
            try
            {
                var stage = dataManager.Stages.GetByID(id);
                if (stage == null) { throw new ObjectNotFoundException(); }
                var response = mapper.Map<Stage, StageDTO>(stage);
                return response;
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Элемент не найден")));
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
        public IHttpActionResult AddStage([FromBody] StageDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stage = mapper.Map<StageDTO, Stage>(model);
            var content = mapper.Map<ContentDTO, ContentStage>(model.Content);

            content.ModifyDate = DateTime.Now;

            stage.Content = content;
            stage.ModifyDate = DateTime.Now;
            stage.Points = 0;
            stage.AllowSkip = false;

            try
            {
                dataManager.Stages.Add(stage);
                dataManager.Save();

                return Ok(stage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка сохранения данных")));
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] StageDTO model)
        {
            try
            {
                var stageOriginal = dataManager.Stages.GetByID(model.Id);
                if (stageOriginal == null) { throw new ObjectNotFoundException(); }

                var stageResult = mapper.Map<StageDTO, Stage>(model, stageOriginal);

                dataManager.Stages.Update(stageResult);
                dataManager.Save();

                return Ok();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Элемент не найден")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Данные не обновлены")));
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var stage = dataManager.Stages.GetByID(id);
                if (stage == null) { throw new ObjectNotFoundException(); }

                dataManager.Stages.Delete(stage);
                dataManager.Save();
                return Ok();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Элемент не найден")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Данные не удалены")));
            }
        }
    }
}