using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class StageDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Point { get; set; }
        public ICollection<MotionDTO> Motions { get; set; }
    }
}
