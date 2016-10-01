using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Areas.Design.Models
{
    public class NewItemViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
    }
}