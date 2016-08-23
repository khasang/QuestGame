using AutoMapper;
using Newtonsoft.Json;
using QuestGame.Domain;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using QuestGame.Common.Interfaces;

namespace QuestGame.WebApi.Controllers
{
    [RoutePrefix("api/Quest")]
    public class QuestController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public QuestController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Route("Get")]
        public IEnumerable<QuestDTO> GetQuest()
        {
            logger.Information("Запрос на все квесты!");

            try
            {
                var quests = dataManager.Quests.GetAll().ToList();

                var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);           
                return response;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }            
        }
    }    
}

    
