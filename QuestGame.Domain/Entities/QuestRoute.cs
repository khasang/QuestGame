using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuestGame.Domain.Entities
{
    public class QuestRoute
    {
        public QuestRoute()
        {
            this.VisitedStages = new List<Stage>();
        }

        public int Id { get; set; }
        public System.DateTime? ModifyDate { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual Quest Quest { get; set; }
        public int QuestId { get; set; }

        [JsonIgnore]
        public ICollection<Stage> VisitedStages { get; set; }

        [JsonIgnore]
        public virtual Stage LastStage { get; set; }
        public int LastStageId { get; set; }
    }
}