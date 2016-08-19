
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class ContentStage : Content
    {
        public virtual Stage Stage { get; set; }
    }
}