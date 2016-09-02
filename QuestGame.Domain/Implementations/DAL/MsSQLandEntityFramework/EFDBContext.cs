using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Implementations
{
    public class EFDBContext : IdentityDbContext<ApplicationUser>, IDBContext
    {
        public EFDBContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public DbSet<ContentQuest> QuestContents { get; set; }
        public DbSet<ContentStage> StageContents { get; set; }

        public DbSet<QuestRoute> QuestRoutes { get; set; }

        public IDbSet<ApplicationUser> GetUsers() { return base.Users; }
        public IDbSet<IdentityRole> GetRoles() { return base.Roles; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public void Save()
        {
            this.SaveChanges();
        }

    }
}
