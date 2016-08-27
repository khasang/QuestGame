using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Implementations
{
    public class EFRoleRepository : IRoleRepository
    {
        IApplicationDbContext dbContext;

        public EFRoleRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return dbContext.GetRoles();
        }

        public IdentityRole GetById(object id)
        {
            return dbContext.GetRoles().Find((string)id);
        }

        public void Add(IdentityRole item)
        {
            dbContext.GetRoles().Add(item);
        }

        public void Update(IdentityRole item)
        {
            dbContext.EntryObj(item);
        }

        public void Delete(IdentityRole item)
        {
            dbContext.GetRoles().Remove(item);
        }

        public void Delete(object id)
        {
            Delete(GetById((string)id));
        }
    }
}
