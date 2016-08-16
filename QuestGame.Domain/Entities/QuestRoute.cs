using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestGame.Domain
{
    public class QuestRoute
    {
        protected DateTime modifyDate;

        private int Id { get; set; }
        private Quest Quest { get; set; }
        private Entities.ApplicationUser User { get; set; }

        public DateTime ModifyDate { get { return this.modifyDate; } set { this.modifyDate = DateTime.Now; } }
    }
}