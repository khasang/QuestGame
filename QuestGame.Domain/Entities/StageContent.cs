
namespace QuestGame.Domain.Entities
{
    public class StageContent : Content
    {
        public virtual Stage Parent { get; set; }
    }
}