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

namespace QuestGame.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(
                    DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Map(m =>
            {
                m.Properties(d => new { d.Email, d.EmailConfirmed, d.PasswordHash,
                    d.SecurityStamp, d.PhoneNumber, d.PhoneNumberConfirmed, d.TwoFactorEnabled,
                    d.LockoutEndDateUtc, d.LockoutEnabled, d.AccessFailedCount, d.UserName
                });
                m.ToTable("AspNetUsers");
            })
            .Map(m =>
            {
                m.Properties(d => new { d.Nik, d.Avatar, d.Losung, d.Contry, d.Rating, d.CountQuestsComplite });
                m.ToTable("AspNetUsersProfile");
            })
            .Ignore( m => new { m.Name, m.LastName })
            ;

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<QuestGame.Domain.Entities.ApplicationUser> ApplicationUsers { get; set; }
    }
}
