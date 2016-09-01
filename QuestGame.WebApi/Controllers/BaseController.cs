using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using QuestGame.WebApi.Attributes;
using QuestGame.WebApi.Models;

namespace QuestGame.WebApi.Controllers
{
    [CustomAuthorize]
    public class BaseController : Controller
    {
        protected IMapper mapper;

        public BaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected UserModel SessionUser
        {
            get
            {
                return Session["User"] as UserModel;
            }
        }
    }
}