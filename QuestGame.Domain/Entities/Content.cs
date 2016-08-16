using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestGame.Domain
{
    public abstract class Content
    {
        protected DateTime modifyDate;

        public virtual int Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public DateTime ModifyDate { get { return this.modifyDate; } set { this.modifyDate = DateTime.Now; } }

        public virtual Object Owner { get; set; }
    }
}