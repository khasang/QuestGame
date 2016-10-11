using QuestGame.Domain.DTO;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Areas.Game.Models;
using QuestGame.WebApi.Attributes;
using QuestGame.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using QuestGame.Common.Helpers;
using QuestGame.WebApi.Constants;

namespace QuestGame.WebApi.Areas.Game.Controllers
{
    public class MainPageController : BaseController
    {
        public MainPageController(IMapper mapper)
            : base(mapper)
        { }

        // GET: Game/MainPage
        public async Task<ActionResult> Index()
        {
            using (var client = RestHelper.Create(SessionUser.Token))
            {
                var response = await client.GetAsync(ApiMethods.QuestGetByActive);

                IEnumerable<QuestViewModels> model = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = ErrorMessages.BadRequest;
                }
                else
                {
                    var answer = await response.Content.ReadAsAsync<IEnumerable<QuestDTO>>();
                    model = mapper.Map<IEnumerable<QuestDTO>, IEnumerable<QuestViewModels>>(answer);
                }

                return View(model);
            }
        }
    }
}