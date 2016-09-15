using AutoMapper;
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
                return null;
            }
        }


        [HttpPut]
        [Route("Update")]
        public void Update(StageDTO model)
        {
            var stageOriginal = dataManager.Stages.GetById(model.Id);
            var stageResult = mapper.Map<StageDTO, Stage>(model, stageOriginal);

            dataManager.Stages.Update(stageResult);
            dataManager.Save();
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(StageDTO stage)
        {
            var model = mapper.Map<StageDTO, Stage>(stage);

            try
            {
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

        [HttpDelete]
        [Route("Delete")]
        public void Delete(int id)
        {
            var stage = dataManager.Stages.GetById(id);

            dataManager.Stages.Delete(stage);
            dataManager.Save();
        }
    }
}
