using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Models
{
    public class SocialUserModel
    {
        public string SocialId { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
    }
}