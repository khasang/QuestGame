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
using System.Net.Http;
using System.Web.Http;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Motion")]
    public class MotionController : ApiController
    {
        IDataManager dataManager;
        IMapper mapper;

        public MotionController(IDataManager dataManager, IMapper mapper)
        {
            this.dataManager = dataManager;
            this.mapper = mapper;
        }


        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(MotionDTO motion)
        {
            var model = mapper.Map<MotionDTO, Motion>(motion);

            try
            {
                dataManager.Motions.Add(model);
                dataManager.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public void Delete(int id)
        {
            var motion = dataManager.Motions.GetById(id);

            dataManager.Motions.Delete(motion);
            dataManager.Save();
        }
    }
}
