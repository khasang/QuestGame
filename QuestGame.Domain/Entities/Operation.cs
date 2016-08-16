using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestGame.Domain
{
    public class Operation
    {
        protected DateTime modifyDate;

        private int Id { get; set; }
        private string Description { get; set; }
        private Stage NextStage { get; set; }
        public DateTime ModifyDate { get { return this.modifyDate; } set { this.modifyDate = DateTime.Now; } }

        public int OwnerId { get; set; }
        public Stage Owner { get; set; }
    }
}