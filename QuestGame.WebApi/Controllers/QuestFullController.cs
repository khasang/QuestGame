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
using System.Web.Configuration;
using System.IO;
using System.Threading.Tasks;
using QuestGame.WebApi.Constants;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/QuestFull")]
    public class QuestFullController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;
        ILoggerService logger;

        public QuestFullController(IDataManager dataManager, IMapper mapper, ILoggerService logger)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
            this.logger = logger;

            //logger.Information("Request | QuestController | from {0} | user: {1}", HttpContext.Current.Request.UserHostAddress, User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<QuestFullDTO> GetAll()
        {
            try
            {
                var quests = dataManager.Quests.GetAll().ToList();
                var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestFullDTO>>(quests);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | GetAll | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        [HttpGet]
        [Route("GetById")]
        public QuestFullDTO GetById(int? id)
        {
            try
            {
                var quest = dataManager.Quests.GetById(id);
                var response = mapper.Map<Quest, QuestFullDTO>(quest);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | GetById | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            } 
        }

        [HttpGet]
        [Route("GetByTitle")]
        public QuestFullDTO GetByTitle(string title)
        {
            try
            {
                var quest = dataManager.Quests.GetByTitle(title);
                var response = mapper.Map<Quest, QuestFullDTO>(quest);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | GetByTitle | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(QuestFullDTO quest)
        {
            try
            {
                var model = mapper.Map<QuestFullDTO, Quest>(quest);

                //var owner = dataManager.UserManager.FindByNameAsync(quest.Owner);
                var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
                if (owner == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                model.Owner = owner;
                model.Date = DateTime.Now;

                dataManager.Quests.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | Create | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(QuestFullDTO quest)
        {
            try
            {
                var questEntity = dataManager.Quests.GetById(quest.Id);
                mapper.Map<QuestFullDTO, Quest>(quest, questEntity);

                var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);

                dataManager.Quests.Update(questEntity);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | Update | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(QuestFullDTO quest)
        {
            try
            {
                var model = mapper.Map<QuestFullDTO, Quest>(quest);

                // Пока не работает. Нужен Id
                //dataManager.Quests.Delete(model);

                dataManager.Quests.DeleteByTitle(model.Title);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | Delete | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            
        }

        [HttpDelete]
        [Route("DelByTitle")]
        public IHttpActionResult DelByTitle(string title)
        {
            try
            {
                dataManager.Quests.DeleteByTitle(title);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | DelByTitle | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [Route("DelById")]
        public IHttpActionResult DelById(int id)
        {
            try
            {
                dataManager.Quests.Delete(id);
                dataManager.Save();
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | DelById | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Загрузка файлов
        /// </summary>
        [HttpPost]
        [Route("UploadFile")]
        [AllowAnonymous]  // На время тестирования
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                var result = await Upload();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("QuestFull | UploadFile | ", ex.ToString());
                return InternalServerError(ex);
            }
        }

        private async Task<string> Upload()
        {
            var content = Request.Content;

            if (!content.IsMimeMultipartContent())
                throw new Exception();

            var path = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["PathToFiles"]);
            var provider = new MultipartFormDataStreamProvider(path);

            var result = await content.ReadAsMultipartAsync(provider);

            if (provider.FileData.Count > 1)
                throw new FileLoadException(ErrorMessages.LoadOnlyOneFile);

            var file = new FileInfo(provider.FileData[0].LocalFileName);
            if (file.Length == 0)
                throw new Exception(ErrorMessages.DefectFile);

            var fileName = provider.FileData[0].Headers.ContentDisposition.FileName.Trim('"');
            var pathName = $"{path}{Path.DirectorySeparatorChar}{fileName}";

            if (File.Exists(pathName))
                throw new Exception(ErrorMessages.ExistsFile);

            try
            {
                file.MoveTo(pathName); // Здесь мы можем настроить какие картинки где хранить
            }
            catch (Exception ex)
            {
                foreach (var fileData in provider.FileData)  // если какие-либо ошибки при перемещении,
                    File.Delete(fileData.LocalFileName);     // то удаляем загруженные файлы

                throw new Exception(ErrorMessages.DefectFile, ex);
            }

            return pathName;
        }
    }    
}

    
