using System.Data.Entity;

namespace QuestGame.Domain.Entities
{
    public class QuestGameContext : DbContext
    {
        public QuestGameContext() :
            base("DefaultConnection")
        { }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public DbSet<QuestContent> QuestContents { get; set; }
        public DbSet<StageContent> StageContents { get; set; }
    }
}
