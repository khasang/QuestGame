using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Models
{
    public class UserViewModel
    {
        [JsonIgnore]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Обращение")]
        public string NickName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }

        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool EmailConfirmed { get; set; }

        public UserProfileViewModel UserProfile { get; set; }

        public bool isExternalLogin { get; set; }

    }
}