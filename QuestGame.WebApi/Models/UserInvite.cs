using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Models
{
    public class UserInvite
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}