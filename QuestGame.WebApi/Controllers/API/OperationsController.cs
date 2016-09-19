using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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

        // GET: api/Operations
        public IEnumerable<OperationDTO> GetOperations()
        {
            try
            {
                var operation = dataManager.Operations.GetAll().ToList();
                var response = mapper.Map<IEnumerable<Operation>, IEnumerable<OperationDTO>>(operation);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка получения данных")));
            }
        }

        [HttpGet]
        [Route("GetById")]
        public OperationDTO GetById(int id)
        {
            try
            {
                var operation = dataManager.Operations.GetByID(id);
                if (operation == null) { throw new ObjectNotFoundException(); }
                var response = mapper.Map<Operation, OperationDTO>(operation);
                return response;
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Элемент не найден")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка получения данных")));
            }
        }

        // Add
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddOperation(OperationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operation = mapper.Map<OperationDTO, Operation>(model);

            try
            {
                dataManager.Operations.Add(operation);
                dataManager.Save();

                return Ok(operation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Ошибка сохранения данных")));
            }
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update(OperationDTO model)
        {
            try
            {
                var operationOriginal = dataManager.Operations.GetByID(model.Id);
                if (operationOriginal == null) { throw new ObjectNotFoundException(); }
                var operationResult = mapper.Map<OperationDTO, Operation>(model, operationOriginal);

                dataManager.Operations.Update(operationResult);
                dataManager.Save();

                return Ok();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Элемент не найден")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Данные не обновлены")));
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var operation = dataManager.Operations.GetByID(id);
                if (operation == null) { throw new ObjectNotFoundException(); }

                dataManager.Operations.Delete(operation);
                dataManager.Save();
                return Ok();
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Элемент не найден")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("Данные не удалены")));
            }
        }
    }
}
