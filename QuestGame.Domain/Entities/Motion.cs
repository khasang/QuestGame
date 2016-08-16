using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Motion
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int? NextStageId { get; set; }
        public virtual Stage NextStage { get; set; }

        public int OwnerStageId { get; set; }
        public virtual Stage OwnerStage { get; set; }

        //public Motion()
        //{
        //    Description = string.Empty;
        //}
    }
}
