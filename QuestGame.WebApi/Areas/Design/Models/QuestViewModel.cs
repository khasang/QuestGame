using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Design.Models
{
    public class QuestViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Название квеста")]
        public string Title { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Дата создания")]
        public DateTime? Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Owner { get; set; }

        public ICollection<StageViewModel> Stages { get; set; }
    }
}