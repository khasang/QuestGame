using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Stage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        
        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }

        [InverseProperty("OwnerStage")]
        public virtual ICollection<Motion> Motions { get; set; }
        
        public Stage()
        {
            //Title = "Stage";
            //Body = string.Empty;
            Motions = new List<Motion>();
        }
    }
}
