using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class QuestDTO
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }
        public bool Active { get; set; }
        public string Owner { get; set; }
        public ICollection<StageDTO> Stages { get; set; }
    }
}
