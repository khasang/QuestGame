using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Models
{
    public class UserProfileViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string avatarUrl { get; set; }

        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }

        [Display(Name = "Пройдено квестов")]
        public int CountCompliteQuests { get; set; }

        [Display(Name = "Зарегистрировался")]
        public DateTime? InviteDate { get; set; }
    }
}