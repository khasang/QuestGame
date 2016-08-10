using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Newtonsoft.Json;

using QuestGame.WebApi.Models;

namespace QuestGame.WebApi.Controllers
{
    public class RegisterWebApiController : ApiController
    {
        public string RegisterUser(UserInvite user)
        {
            return JsonConvert.SerializeObject(user);
        }
    }
}
