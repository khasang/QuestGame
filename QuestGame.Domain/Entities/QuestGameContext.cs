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

        protected override void OnModelCreating(
                            DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Quest>().HasOptional(u => u.QuestContent).WithRequired();
            modelBuilder.Entity<Stage>().HasOptional(u => u.StageContent).WithRequired();


            base.OnModelCreating(modelBuilder);
        }
    }
}
