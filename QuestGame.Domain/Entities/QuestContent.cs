using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class QuestContent : Content
    {
        [Key]
        [ForeignKey("Parent")]
        public new int Id { get; set; }
        public virtual Quest Parent { get; set; }
    }
}