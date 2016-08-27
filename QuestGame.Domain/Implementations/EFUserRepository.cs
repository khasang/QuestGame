using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Implementations
{
    public class EFUserRepository : IUserRepository
    {
        IApplicationDbContext dbContext;

        public EFUserRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;           
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return dbContext.GetUsers(); ;
        }

        public ApplicationUser GetById(object id)
        {
            return dbContext.GetUsers().Find((string)id);
        }

        public void Add(ApplicationUser item)
        {
            dbContext.GetUsers().Add(item);
        }

        public void Update(ApplicationUser item)
        {
            dbContext.EntryObj(item);
        }

        public void Delete(ApplicationUser item)
        {
            dbContext.GetUsers().Remove(item);
        }

        public void Delete(object id)
        {
            Delete(GetById((string)id));
        }
    }
}
