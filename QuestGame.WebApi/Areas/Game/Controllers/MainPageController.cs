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
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", SessionUser.Token);

                var response = await client.GetAsync(@"api/Quest/GetAll");

                IEnumerable<QuestViewModel> model = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "Неудачный запрос!";
                }
                else
                {
                    var answer = await response.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

                    model = mapper.Map<IEnumerable<QuestDTO>, IEnumerable<QuestViewModel>>(answer);
                }

                return View(model);
            }
        }
    }
}