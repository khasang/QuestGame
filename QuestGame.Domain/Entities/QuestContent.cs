namespace QuestGame.Domain.Entities
{
    public class QuestContent : Content
    {
//        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }
    }
}