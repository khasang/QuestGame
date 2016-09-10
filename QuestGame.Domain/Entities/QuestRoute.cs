using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuestGame.Domain.Entities
{
    public class QuestRoute
    {
        public int Id { get; set; }
        public System.DateTime? ModifyDate { get; set; }

        public int QuestId { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }

        [JsonIgnore]
        public virtual Quest Quest { get; set; }

        [JsonIgnore]
        public virtual ICollection<Stage> VisitedStages { get; set; }

        [JsonIgnore]
        public virtual Stage LastStage { get; set; }
    }
}