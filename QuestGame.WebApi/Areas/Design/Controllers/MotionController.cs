using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Game.Models;
using QuestGame.WebApi.Constants;
using QuestGame.WebApi.Controllers;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class MotionController : BaseController
    {
        public MotionController(IMapper mapper)
            : base(mapper)
        { }

        // GET: Game/Motion
        public async Task<ActionResult> Index(int id)
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.MotionGetByStageId + id);
                var answer = await response.Content.ReadAsAsync<IEnumerable<MotionDTO>>();
                var model = mapper.Map<IEnumerable<MotionDTO>, IEnumerable<MotionViewModel>>(answer);
                return View(model);
            }
        }
    }
}