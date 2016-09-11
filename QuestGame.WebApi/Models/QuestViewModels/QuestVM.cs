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
        [HiddenInput(DisplayValue = false)]
        public bool Id { get; set; }

        [Display(Name = "Опубликовать квест")]
        public bool Active { get; set; }

        [JsonIgnore]
        [Display(Name = "Картинка для описания")]
        public HttpPostedFileBase File { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Image { get; set; }

        [Required(ErrorMessage = "Назваие не может быть пустым")]
        [Display(Name = "Название Квеста")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 символов")]
        public string Title { get; set; }

        [Display(Name = "Введите описание")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}