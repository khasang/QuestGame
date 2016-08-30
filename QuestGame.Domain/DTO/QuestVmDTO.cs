using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class QuestVmDTO
    {
        public bool Active { get; set; }
        public string Title { get; set; }
        public int CountComplite { get; set; }
        public int? Rate { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
