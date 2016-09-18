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
            var quests = dataManager.Quests.GetAll().ToList();

            var response = mapper.Map<IEnumerable<Quest>, IEnumerable<QuestFullDTO>>(quests);
            return response;            
        }

        [HttpGet]
        [Route("GetById")]
        public QuestFullDTO GetById(int id)
        {
            var quest = dataManager.Quests.GetById(id);

            var response = mapper.Map<Quest, QuestFullDTO>(quest);
            return response;
        }

        [HttpGet]
        [Route("GetByTitle")]
        public QuestFullDTO GetByTitle(string title)
        {
            var quest = dataManager.Quests.GetByTitle(title);

            var response = mapper.Map<Quest, QuestFullDTO>(quest);
            return response;
        }

        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create(QuestFullDTO quest)
        {
            var model = mapper.Map<QuestFullDTO, Quest>(quest);

            try
            {
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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("Update")]
        public void Update(QuestFullDTO quest)
        {
            var questEntity = dataManager.Quests.GetById(quest.Id);
            mapper.Map<QuestFullDTO, Quest>(quest, questEntity);

            var owner = dataManager.Users.GetAll().FirstOrDefault(x => x.UserName == quest.Owner);
            if (owner == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            dataManager.Quests.Update(questEntity);
            dataManager.Save();
        }

        [HttpDelete]
        [Route("Delete")]
        public void Delete(QuestFullDTO quest)
        {
            var model = mapper.Map<QuestFullDTO, Quest>(quest);

            // Пока не работает. Нужен Id
            //dataManager.Quests.Delete(model);

            if (model != null && !string.IsNullOrEmpty(model.Title))
            {
                dataManager.Quests.DeleteByTitle(model.Title);
                dataManager.Save();
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
            catch
            {
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
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Загрузка файлов
        /// </summary>
        [HttpPost]
        [Route("UploadFile")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                UploadFile(Request.Content);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private void UploadFile(HttpContent content)
        {
            if (!content.IsMimeMultipartContent())
            {
                throw new Exception();
            }

            var provider = new MultipartFormDataStreamProvider(WebConfigurationManager.AppSettings["pathToFiles"]);

            content.ReadAsMultipartAsync(provider).Wait();

            if (provider.FileData.Count == 1)
            {
                var file = new FileInfo(provider.FileData[0].LocalFileName);

                if (file.Length != 0)
                {
                    string name = provider.FileData[0].Headers.ContentDisposition.FileName.Trim('"');
                    int pos = name.LastIndexOfAny(new[] { '\\', '/' }) + 1;
                    name = file.DirectoryName + Path.DirectorySeparatorChar + name.Substring(pos);

                    if (File.Exists(name))
                        throw new Exception("Сертификат с таким имененем уже загружен");
                    else
                    {
                        try
                        {
                            file.MoveTo(name);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Файл сертификата поврежден");
                        }
                    }
                }
            }
            else
                throw new Exception("Доступна одновременная загрузка только одного сертификата");

            //если какие-либо ошибки то удаляем загруженные файлы
            foreach (var fileData in provider.FileData)
            {
                File.Delete(fileData.LocalFileName);
            }
        }
    }    
}

    
