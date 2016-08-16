using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace QuestGame.Domain.Entities
{
    public class QuestGameContext : DbContext
    {
        public QuestGameContext():
            base("DefaultConnection")
        { }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Operation> Operations { get; set; }

    }
}
