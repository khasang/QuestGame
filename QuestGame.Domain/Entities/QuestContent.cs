using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class QuestContent : Content
    {
        public virtual Quest Quest { get; set; }
    }
}