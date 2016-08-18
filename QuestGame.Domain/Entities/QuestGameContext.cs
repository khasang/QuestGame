using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;

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

        protected override void OnModelCreating(
                            DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stage>().HasRequired(s => s.StageContent).WithRequiredPrincipal(ss => ss.Stage);
            modelBuilder.Entity<Quest>().HasRequired(s => s.QuestContent).WithRequiredPrincipal(ss => ss.Quest);

            base.OnModelCreating(modelBuilder);
        }
    }
}
