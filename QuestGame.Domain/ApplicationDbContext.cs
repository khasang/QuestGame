using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuestGame.Domain.Interfaces;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace QuestGame.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDBContext
    {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public DbSet<ContentQuest> QuestContents { get; set; }
        public DbSet<ContentStage> StageContents { get; set; }

        public DbSet<QuestRoute> QuestRoutes { get; set; }

        public IDbSet<ApplicationUser> GetUsers() { return base.Users; }
        public IDbSet<IdentityRole> GetRoles() { return base.Roles; }

        public void Save()
        {
            this.SaveChanges();
        }


        protected override void OnModelCreating(
                    DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Map(m =>
            {
                m.Properties(d => new
                {
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
                m.Properties(d => new { d.Name, d.LastName, d.Bithday, d.Avatar, d.Country, d.Rating, d.CountQuestsComplite, d.AddDate });
                m.ToTable("AspNetUsersProfile");
            })
            .Ignore(d => d.Token);

            //modelBuilder.Entity<ApplicationUser>().HasMany(s => s.QuestsRoutes).WithRequired(q => q.User).WillCascadeOnDelete(true);
            modelBuilder.Entity<ApplicationUser>().HasMany(s => s.Quests).WithRequired(q => q.User).WillCascadeOnDelete(true);

            modelBuilder.Entity<Quest>().HasRequired(s => s.Content).WithRequiredPrincipal(ss => ss.Owner).WillCascadeOnDelete(true);
            modelBuilder.Entity<Quest>().HasMany(s => s.Stages).WithRequired(q => q.Quest).WillCascadeOnDelete(true);

            modelBuilder.Entity<Stage>().HasRequired(s => s.Content).WithRequiredPrincipal(ss => ss.Owner).WillCascadeOnDelete(true);
            modelBuilder.Entity<Stage>().HasMany(o => o.Operations).WithRequired(s => s.Stage).WillCascadeOnDelete(true);

            modelBuilder.Entity<Operation>().HasRequired(s => s.Stage).WithMany(o => o.Operations).HasForeignKey(k => k.StageId).WillCascadeOnDelete(true);

            //modelBuilder.Entity<QuestRoute>().HasMany(o => o.VisitedStages).WithOptional().WillCascadeOnDelete(false);
            modelBuilder.Entity<QuestRoute>().HasRequired(r => r.LastStage).WithRequiredDependent().WillCascadeOnDelete(false);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<QuestRoute>()
                .HasMany(s => s.VisitedStages)
                .WithMany(q => q.Routes)
                .Map( m =>
                {
                    m.ToTable("VisitedStages");
                    m.MapLeftKey("QuestRouteId");
                    m.MapRightKey("StageId");
                }
           );

            base.OnModelCreating(modelBuilder);
        }

        public void Attach(Quest q)
        {
            throw new NotImplementedException();
        }
    }
}
