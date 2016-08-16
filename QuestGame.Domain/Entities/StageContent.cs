using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain
{
    public class StageContent : Content
    {
        [Key]
        [ForeignKey("Owner")]
        public override int Id { get; set; }

        public new Stage Owner { get; set; }
    }
}