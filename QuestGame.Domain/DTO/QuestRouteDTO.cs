using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class QuestRouteDTO
    {
        public int Id { get; set; }
        public int Quest { get; set; }
        public string User { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
