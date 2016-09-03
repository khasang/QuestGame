using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Areas.Game.Models
{
    public class QuestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Owner { get; set; }
        public ICollection<StageViewModel> Stages { get; set; }
    }

    public class NewQuestViewModel
    {
        public string Title { get; set; }
        public string Owner { get; set; }
    }

    public class StageViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ICollection<MotionViewModel> Motions { get; set; }
    }

    public class MotionViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int NextStageId { get; set; }
    }
}