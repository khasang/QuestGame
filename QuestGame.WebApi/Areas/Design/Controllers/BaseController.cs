using AutoMapper;
using QuestGame.Common;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Design.Models;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Design.Controllers
{
    public class BaseController : Controller
    {
        UserModel user;

        public BaseController()
        {
            user = Session["User"] as UserModel;
        }

    }
}