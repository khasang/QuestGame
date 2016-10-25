using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Адрес электронной почты")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Как к Вам обращаться? Введите Ваше ФИО или Никнейм")]
        public string NickName { get; set; }

        [JsonIgnore]
        [Display(Name = "Аватар")]
        public HttpPostedFileBase File { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string avatarUrl { get; set; }
    }
}
