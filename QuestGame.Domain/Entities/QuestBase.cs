using System;
using System.Collections.Generic;

namespace QuestGame.Domain.Entities
{
    public abstract class QuestBase
    {
        public QuestBase()
        {
            this.Active = true;
            this.CountComplite = 0;
            this.Rate = 0;
            this.Stages = new List<Stage>();
        }

        public int Id { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public short CountComplite { get; set; }
        public Nullable<short> Rate { get; set; }
        public System.DateTime AddDate { get; set; }
        public System.DateTime ModifyDate { get; set; }

        public virtual ICollection<Stage> Stages { get; set; }
        public virtual QuestContent QuestContent { get; set; }

    }

}