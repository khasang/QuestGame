using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Areas.Design.Models
{
    public class QuestViewModel
    {
        public int Id { get; set; }

        [Display(Name="Название квеста")]
        public string Title { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime Date { get; set; }

        [Display(Name = "Владелец")]
        public string Owner { get; set; }
        public ICollection<StageViewModel> Stages { get; set; }
    }
}