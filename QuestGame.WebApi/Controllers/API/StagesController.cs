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
            var stages = dataManager.Stages.GetAll().ToList();
            var response = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stages);

            return response;
        }

        // Add
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddStage( StageVM stageVM )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stage = mapper.Map<StageVM, Stage>(stageVM);
            var content = mapper.Map<StageVM, ContentStage>(stageVM);

            stage.Content = content;

            dataManager.Stages.Add(stage);
            dataManager.Save();

            return Ok(stage);
        }


    }
}