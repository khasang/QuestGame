using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestGame.Domain.Entities
{
    public class Operation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ModifyDate { get; set; }
        //public int RedirectToStage { get; set; }
        public int StageId { get; set; }

        [JsonIgnore]
        public virtual Stage Stage { get; set; }

        public Operation()
        {
            this.ModifyDate = DateTime.Now;
        }
    }
}