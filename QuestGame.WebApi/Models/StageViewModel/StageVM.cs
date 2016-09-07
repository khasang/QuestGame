using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Models
{
    public class StageVM
    {
        [Required(ErrorMessage = "Назваие не может быть пустым")]
        [Display(Name = "Название Квеста")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 символов")]
        public string Title { get; set; }

        [Display(Name = "Введите описание")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public int QuestId { get; set; }
    }
}