using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.Domain.Entities
{
    [Serializable]
    public class Quest
    {
        public Quest()
        {
            this.Active = true;
            this.CountComplite = 0;
            this.Rate = 0;
            this.AddDate = DateTime.Now;
            this.ModifyDate = DateTime.Now;
            this.Stages = new List<Stage>();
        }

        public int Id { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public int CountComplite { get; set; }
        public int? Rate { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifyDate { get; set; }

        [Required]
        public virtual ContentQuest Content { get; set; }

        public virtual ICollection<Stage> Stages { get; set; }
    }
}