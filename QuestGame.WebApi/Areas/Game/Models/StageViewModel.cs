using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Areas.Game.Models
{
    public class StageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Body { get; set; }

        [Display(Name = "Стоимость")]
        public int Point { get; set; }

        public int QuestId { get; set; }

        public ICollection<MotionViewModel> Motions { get; set; }
    }
}