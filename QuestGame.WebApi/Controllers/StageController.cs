using AutoMapper;
using QuestGame.Common.Interfaces;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
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
