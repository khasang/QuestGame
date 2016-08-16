using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain
{
    public class QuestContent : Content
    {
        [Key]
        [ForeignKey("Owner")]
        public override int Id { get; set; }
        public new Quest Owner  { get; set; }
    }
}