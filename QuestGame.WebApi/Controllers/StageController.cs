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
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(StageDTO model)
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
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(StageDTO stage)
        {
            try
            {
                var model = mapper.Map<StageDTO, Stage>(stage);

                dataManager.Stages.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

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
                return Ok(stage);
            }
            catch (ObjectNotFoundException ex)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
