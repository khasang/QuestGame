using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Models
{
    public class QuestVM
    {

        [Display(Name = "Опубликовать квест")]
        public bool Active { get; set; }

        [JsonIgnore]
        [Display(Name = "Картинка для описания")]
        public HttpPostedFileBase File { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Введите название Квеста")]
        public string Title { get; set; }

        [Display(Name = "Введите описание")]
        public string Text { get; set; }
    }
}