namespace QuestGame.Domain.Entities
{
    public class QuestRoute
    {
        private int Id { get; set; }
        private Quest Quest { get; set; }
        private Entities.ApplicationUser User { get; set; }
        public System.DateTime ModifyDate { get; set; }
    }
}