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


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
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
                    d.UserName
                });
                m.ToTable("AspNetUsers");
            })
            .Map(m =>
            {
                m.Properties(d => new { d.Name, d.LastName, d.Avatar, d.Contry, d.Rating, d.CountQuestsComplite, d.AddDate });
                m.ToTable("AspNetUsersProfile");
            })
            .Ignore(d => d.Token);

            modelBuilder.Entity<Stage>().HasRequired(s => s.Content).WithRequiredPrincipal(ss => ss.Stage);
            modelBuilder.Entity<Quest>().HasRequired(s => s.Content).WithRequiredPrincipal(ss => ss.Quest);

            base.OnModelCreating(modelBuilder);
        }
    }
}
