using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using QuestGame.Domain.EntityConfigurations;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Motion> Motions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Image> Images { get; set; }
        public IDbSet<ApplicationUser> GetUsers() { return base.Users; }
        public IDbSet<IdentityRole> GetRoles() { return base.Roles; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            var ttr = new ApplicationDbContext();
            return ttr;
        }

        public void EntryObj<T>(T entity) where T : class
        {
            base.Entry(entity).State = EntityState.Modified;
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Здесь подключаем настройки связей сущностей между собой

            modelBuilder.Configurations.Add(new ApplicationUserMapper());
            modelBuilder.Configurations.Add(new MotionMapper());
            modelBuilder.Configurations.Add(new QuestMapper());
            modelBuilder.Configurations.Add(new StageMapper());
            modelBuilder.Configurations.Add(new UserProfileMapper());

            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<ApplicationUser>().HasOptional(x => x.UserProfileId).WithRequired();
        }
    }
}
