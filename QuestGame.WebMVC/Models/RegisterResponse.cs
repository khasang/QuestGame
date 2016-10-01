using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Models
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
        public string ErrorMessage { get; set; }
    }
}