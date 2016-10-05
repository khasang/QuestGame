using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.WebMVC.Models
{
    public class ApplicationUserViewModel
    {
        [Display(Name = "Как к вам обращаться, введите ФИО или Никнейм")]
        public string NickName { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }

        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        public UserProfileViewModel UserProfile { get; set; }
    }
}