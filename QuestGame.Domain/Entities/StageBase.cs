using System;
using System.Collections.Generic;

namespace QuestGame.Domain.Entities
{
    public abstract class StageBase
    {
        public StageBase()
        {
            this.Points = 0;
            this.AllowSkip = false;
            this.Operations = new List<Operation>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<short> Points { get; set; }
        public bool AllowSkip { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public int QuestId { get; set; }

        public virtual Quest Quest { get; set; }
        public virtual StageContent StageContent { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}