using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<Quest> Quests { get; set; }
        DbSet<Stage> Stages { get; set; }
        DbSet<Motion> Motions { get; set; }

        IDbSet<ApplicationUser> GetUsers();
        IDbSet<IdentityRole> GetRoles();

        void SaveChanges();
        void EntryObj<T>(T entity) where T : class;
    }
}
