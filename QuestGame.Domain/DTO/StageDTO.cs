using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class StageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Points { get; set; }
        public bool AllowSkip { get; set; }
        public DateTime ModifyDate { get; set; }
        public int QuestId { get; set; }
        public virtual ContentDTO Content { get; set; }

        public ICollection<OperationDTO> Operations;
    }
}
