
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class StageContent : Content
    {
        [Key]
        [ForeignKey("Parent")]
        public new int Id { get; set; }
        public virtual Stage Parent { get; set; }
    }
}