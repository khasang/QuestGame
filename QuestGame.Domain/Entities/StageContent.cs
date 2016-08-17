
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestGame.Domain.Entities
{
    public class StageContent : Content
    {
        public virtual Stage Stage { get; set; }
    }
}