using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Areas.Design.Models
{
    public class StageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название сцены")]
        public string Title { get; set; }

        [Display(Name = "Тег")]
        public string Tag { get; set; }

        [Display(Name = "Описание сцены")]
        public string Body { get; set; }

        [Display(Name = "Количество баллов")]
        public int Point { get; set; }

        public int QuestId { get; set; }

        public ICollection<MotionViewModel> Motions { get; set; }
    }
}