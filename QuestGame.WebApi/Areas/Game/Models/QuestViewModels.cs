using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebApi.Areas.Game.Models
{
    public class QuestViewModels
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Рейтинг")]
        public int Rate { get; set; }

        [Display(Name = "Активирован")]
        public bool Active { get; set; }

        [Display(Name = "Автор")]
        public string Owner { get; set; }

        public IDictionary<int, string> Stages { get; set; }

        public QuestViewModels()
        {
            Stages = new Dictionary<int, string>();
        }
    }

    public class StageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Тело")]
        public string Body { get; set; }

        [Display(Name = "Бонус")]
        public int Point { get; set; }

        public int QuestId { get; set; }
        public IDictionary<int, string> Motions { get; set; }

        public StageViewModel()
        {
            Motions = new Dictionary<int, string>();
        }
    }

    public class MotionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public int OwnerStageId { get; set; }

        public int? NextStageId { get; set; }
    }
}