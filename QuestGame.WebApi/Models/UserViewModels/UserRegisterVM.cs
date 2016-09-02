using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Models.UserViewModels
{
    public class UserRegisterVM
    {
        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }

        [Display(Name = "Ваше Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName{ get; set; }

        [DataType(DataType.Date)]
    //        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        public DateTime? Bithday { get; set; }

        [Display(Name = "Аватар")]
        public string Avatar { get; set; }

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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
