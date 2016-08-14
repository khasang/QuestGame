using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.WebApi.Models
{
    public class UserLogin
    {
        public string grant_type = "password";

        [Required(ErrorMessage = "Не указан Email")]
        public string username { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}