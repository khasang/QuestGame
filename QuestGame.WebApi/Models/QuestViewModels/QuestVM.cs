using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Models
{
    public class QuestVM
    {

        [Display(Name = "Опубликовать квест")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Введите название Квеста")]
        public string Title { get; set; }

        [Display(Name = "Введите описание")]
        public string Text { get; set; }
    }
}