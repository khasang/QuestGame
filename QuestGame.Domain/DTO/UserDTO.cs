﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        //public string Token { get; set; }

        public UserProfileDTO UserProfile { get; set; }
    }
}
