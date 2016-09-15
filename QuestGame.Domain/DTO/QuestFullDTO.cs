using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class QuestFullDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }
        public bool Active { get; set; }
        public string Owner { get; set; }
        public ICollection<StageFullDTO> Stages { get; set; }
        public string Img { get; set; }
    }

    public class StageFullDTO
    {
        int Id { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Point { get; set; }
        public ICollection<MotionDTO> Motions { get; set; }
    }
}
