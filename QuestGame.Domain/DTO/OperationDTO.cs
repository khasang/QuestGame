using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class OperationDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ModifyDate { get; set; }
        public int RedirectToStage { get; set; }
        public int StageId { get; set; }
        public string StageTitle { get; set; }
    }
}
