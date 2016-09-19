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
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace QuestGame.WebApi.Controllers
{
    //[Authorize]
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

            logger.Information("Request | QuestController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<QuestDTO> GetAll()
        {
            var quests = dataManager.Quests.GetAll().ToList();

            var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(quests);
            return response;
        }

        [HttpGet]
        [Route("GetById")]
        public QuestDTO GetById(int id)
        {
            var quest = dataManager.Quests.GetById(id);

            var response = mapper.Map<Quest, QuestDTO>(quest);
            return response;
        }

        [HttpGet]
        [Route("Details")]
        public IEnumerable<StageDTO> Details(string title)
        {

            //______________________________________________________________________________________________________________________
            //Нужно ли открывать соединение? как возвращать id интовый???

            //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //using (var connection = new SqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();
            //    }
            //    catch (SqlException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        throw;
            //    }
            //    Console.WriteLine("Подключение открыто");

            //    var procedureName = "GetIdByQuestTitle";
            //    var command = new SqlCommand(procedureName, connection);

            //    command.CommandType = CommandType.StoredProcedure;

            //    var param = new SqlParameter
            //    {
            //        ParameterName = "title",
            //        Value = title
            //    };

            //    command.Parameters.Add(param);
            //    var questId = command.ExecuteReader();
                //______________________________________________________________________________________________________________________


                //int questId = dataManager.Quests.GetIdByTitle(title);                          //заменить метод GetIdByTitle на хранимую процедуру и вызвать ее тут же
                var stages = dataManager.Stages.GetStagesByQuestId(questId).ToList();

                var response = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stages);

                return response;
            }
        }

        [HttpGet]
        [Route("StageDetails")]
        public IEnumerable<MotionDTO> StageDetails(string title)
        {
            int stageId = dataManager.Stages.GetIdByTitle(title);
            var motions = dataManager.Motions.GetMotionsByStageId(stageId).ToList();

            var response = mapper.Map<IEnumerable<Motion>, IEnumerable<MotionDTO>>(motions);

            return response;
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(QuestFullDTO quest)
        {
            var model = mapper.Map<QuestFullDTO, Quest>(quest);

            var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.Owner = owner;
            model.Date = DateTime.Now;

            dataManager.Quests.Add(model);
            dataManager.Save();
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public void Update(QuestDTO quest)
        {
            var questEntity = dataManager.Quests.GetById(quest.Id);
            var model = mapper.Map<QuestDTO, Quest>(quest, questEntity);

            var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            model.Owner = owner;
            dataManager.Quests.Update(model);
            dataManager.Save();
        }

        //[HttpDelete]
        //[Route("Delete")]
        //public void Delete(QuestDTO quest)
        //{
        //    var model = mapper.Map<QuestDTO, Quest>(quest);

        //    // Пока не работает. Нужен Id
        //    //dataManager.Quests.Delete(model);
        //    //dataManager.Save();

        //    if (model != null && !string.IsNullOrEmpty(model.Title))
        //    {
        //        dataManager.Quests.DeleteByTitle(model.Title);
        //        dataManager.Save();
        //    }
        //}

        [HttpDelete]
        [Route("DelByTitle")]
        public void DelByTitle(string title)
        {
            //var model = mapper.Map<QuestFullDTO, Quest>(title);

            if (title != null && !string.IsNullOrEmpty(title))
            {
                dataManager.Quests.DeleteByTitle(title);
                dataManager.Save();
            }
        }
    }
}


