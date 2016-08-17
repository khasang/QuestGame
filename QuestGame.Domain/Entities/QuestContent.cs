namespace QuestGame.Domain.Entities
{
    public class QuestContent : Content
    {
        public virtual Quest Parent { get; set; }
    }
}