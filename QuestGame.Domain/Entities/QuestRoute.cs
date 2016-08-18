namespace QuestGame.Domain.Entities
{
    public class QuestRoute
    {
        public int Id { get; set; }
        public Quest Quest { get; set; }
        public Entities.ApplicationUser User { get; set; }
        public System.DateTime ModifyDate { get; set; }
    }
}