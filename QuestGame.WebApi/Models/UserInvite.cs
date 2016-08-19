using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.WebApi.Models
{
    public class UserInvite
    {
        [Display(Name = "Ваше Имя")]
        public string Name  { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Аватар")]
        public string Avatar { get; set; }

        [Display(Name = "Подпись")]
        public string Losung { get; set; }

        [Display(Name = "Страна")]
        public string Contry { get; set; }

        [Display(Name = "Email адрес регистрации")]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}