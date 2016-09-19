using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class Stage
    {
        public Stage()
        {
            this.Operations = new List<Operation>();
            this.Routes = new List<QuestRoute>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public int? Points { get; set; }
        public bool AllowSkip { get; set; }
        public DateTime ModifyDate { get; set; }

        [JsonIgnore]
        public virtual Quest Quest { get; set; }
        public int QuestId { get; set; }

        [JsonIgnore]
        public ICollection<QuestRoute> Routes { get; set; }

        [Required]
        [JsonIgnore]
        public virtual ContentStage Content { get; set; }

        [JsonIgnore]
        [InverseProperty("Stage")]
        public virtual ICollection<Operation> Operations { get; set; }
    }
}