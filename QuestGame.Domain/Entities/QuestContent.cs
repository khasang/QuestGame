using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class QuestContent : Content
    {
        //public int QuestId { get; set; }
        public virtual Quest Parent { get; set; }
    }
}