using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Models.UserViewModels
{
    public class UserProfileVM
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime? Bithday { get; set; }

        [Display(Name = "Аватар")]
        public string Avatar { get; set; }

        [Display(Name = "Страна")]
        public string Contry { get; set; }

        [Display(Name = "Email адрес регистрации")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Token { get; set; }
    }
}