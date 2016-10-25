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
        public string UserId { get; set; }

        public string avatarUrl { get; set; }

        [JsonIgnore]
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }

        [JsonIgnore]
        [Display(Name = "Пройдено квестов")]
        public int CountCompliteQuests { get; set; }

        [JsonIgnore]
        [Display(Name = "Зарегистрировался")]
        public DateTime? InviteDate { get; set; }
    }
}