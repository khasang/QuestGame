using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.WebApi.Models
{
    public class UserViewModel
    {
        [JsonIgnore]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Обращение")]
        public string NickName { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }

        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        public UserProfileViewModel UserProfile { get; set; }

        [JsonIgnore]
        public IEnumerable<UserQuestsViewModel> Quests { get; set; }

    }
}