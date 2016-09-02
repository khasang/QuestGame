﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.Domain.Entities
{
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

        public string UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }

        [Required]
        //[JsonIgnore]
        public virtual ContentQuest Content { get; set; }

        [JsonIgnore]
        public virtual ICollection<Stage> Stages { get; set; }
    }
}