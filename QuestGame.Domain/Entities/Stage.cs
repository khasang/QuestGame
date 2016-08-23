using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.Domain.Entities
{
    public class Stage
    {
        public Stage()
        {
            this.Points = 0;
            this.AllowSkip = false;
            this.Operations = new List<Operation>();
            this.ModifyDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? Points { get; set; }
        public bool AllowSkip { get; set; }
        public DateTime ModifyDate { get; set; }

        public int QuestId { get; set; }

        [JsonIgnore]
        public virtual Quest Quest { get; set; }

        [Required]
        [JsonIgnore]

        public virtual ContentStage Content { get; set; }
        [JsonIgnore]

        public virtual ICollection<Operation> Operations { get; set; }
    }
}