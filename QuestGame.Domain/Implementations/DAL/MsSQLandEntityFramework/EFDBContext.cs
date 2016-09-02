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

        //public override IDbSet<ApplicationUser> Users { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public void Save()
        {
            this.SaveChanges();
        }


        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Map(m =>
            {
                m.Properties(d => new {
                    d.Email,
                    d.EmailConfirmed,
                    d.PasswordHash,
                    d.SecurityStamp,
                    d.PhoneNumber,
                    d.PhoneNumberConfirmed,
                    d.TwoFactorEnabled,
                    d.LockoutEndDateUtc,
                    d.LockoutEnabled,
                    d.AccessFailedCount,
                    d.UserName,
                    d.Identificator
                });
                m.ToTable("AspNetUsers");
            })
            .Map(m =>
            {
                m.Properties(d => new { d.Name, d.LastName, d.Avatar, d.Contry, d.Rating, d.CountQuestsComplite, d.AddDate });
                m.ToTable("AspNetUsersProfile");
            })
            .Ignore(d => d.Token);

            //modelBuilder.Entity<ApplicationUser>().HasMany(p => p.Quests).WithRequired(p => p.User).WillCascadeOnDelete(true);
            //modelBuilder.Entity<ApplicationUser>().HasMany(p => p.QuestsRoutes).WithRequired(p => p.User).WillCascadeOnDelete(true);

            //modelBuilder.Entity<Quest>().HasRequired(s => s.User);
            modelBuilder.Entity<Quest>().HasMany(s => s.Stages).WithRequired(q => q.Quest).WillCascadeOnDelete(true);
            modelBuilder.Entity<Quest>().HasRequired(s => s.Content).WithRequiredPrincipal(ss => ss.Quest).WillCascadeOnDelete(true);

            modelBuilder.Entity<Stage>().HasRequired(s => s.Content).WithRequiredPrincipal(ss => ss.Stage).WillCascadeOnDelete(true);
            modelBuilder.Entity<Stage>().HasMany(p => p.Operations).WithRequired(p => p.Stage).WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
