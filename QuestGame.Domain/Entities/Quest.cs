using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Stage> Stages { get; set; }
        public Quest()
        {
            //Title = "Quest";
            //Date = DateTime.Now;
            Stages = new List<Stage>();
        }
    }
}
