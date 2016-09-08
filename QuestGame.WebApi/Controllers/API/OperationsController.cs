using AutoMapper;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuestGame.WebApi.Controllers.API
{
    [RoutePrefix("api/Operations")]
    public class OperationsController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public OperationsController(IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }

        // Add
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddOperations(OperationsVM operationVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operations = mapper.Map<OperationsVM, Operation>(operationVM);

            dataManager.Operations.Add(operations);
            dataManager.Save();

            return Ok(operations);
        }
    }
}
