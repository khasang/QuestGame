using AutoMapper;
using QuestGame.Common.Interfaces;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using QuestGame.WebApi.Constants;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Stage")]
    public class StageController : BaseController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public StageController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;

            logger.Information("Request | StageController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<StageDTO> GetAll()
        {
            try
            {
                var stages = dataManager.Stages.GetAll().ToList();
                var model = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stages);
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | GetAll | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public StageDTO GetById(int id)
        {
            try
            {
                var stage = dataManager.Stages.GetById(id);
                var model = mapper.Map<Stage, StageDTO>(stage);
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | GetById | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("GetByQuestId")]
        public IEnumerable<StageDTO> GetByQuestId(int id)
        {
            try
            {
                var stage = dataManager.Stages.GetByQuestId(id).ToList();
                var model = mapper.Map<IEnumerable<Stage>, IEnumerable<StageDTO>>(stage);
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | GetByQuestId | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(StageDTO stage)
        {
            try
            {
                var model = mapper.Map<StageDTO, Stage>(stage);
                var owner = dataManager.Quests.GetById(stage.QuestId);
                model.Quest = owner;
                model.Cover = new Image
                {
                    Name = ConfigSettings.GetServerFilePath(ConfigSettings.NoImage),
                    Prefix = string.Empty
                };

                dataManager.Stages.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | Create | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(StageDTO stage)
        {
            try
            {
                var stageEntity = dataManager.Stages.GetById(stage.Id);
                var model = mapper.Map<StageDTO, Stage>(stage, stageEntity);

                model.Cover = new Image
                {
                    Name = stage.Cover,
                    Prefix = ConfigSettings.StagePrefixFile,
                };

                var owner = dataManager.Quests.GetById(stage.QuestId);

                model.Quest = owner;
                dataManager.Stages.Update(model);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | Update | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            } 
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(StageDTO quest)
        {
            try
            {
                var model = mapper.Map<StageDTO, Stage>(quest);
                dataManager.Stages.Delete(model.Id);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | Delete | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            } 
        }

        [HttpDelete]
        [Route("DelById")]
        public IHttpActionResult DelById(int? id)
        {
            try
            {
                dataManager.Stages.Delete(id);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | DelById | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        /// <summary>
        /// Загрузка обложки квеста
        /// </summary>
        [HttpPost]
        [Route("UploadFile")]
        public async Task<string> UploadFile()
        {
            try
            {
                var result = await Upload(ConfigSettings.StagePrefixFile);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Stage | UploadFile | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
