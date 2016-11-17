using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Models
{
    public class UserProfileViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        public string AvatarUrl { get; set; }

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