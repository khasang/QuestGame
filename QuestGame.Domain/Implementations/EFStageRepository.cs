using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;

namespace QuestGame.Domain.Implementations
{
    public class EFStageRepository : IStageRepository
    {
        private ApplicationDbContext dbContext;

        public EFStageRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Stage item)
        {
            dbContext.Stages.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetById((int)id));
        }

        public void Delete(Stage item)
        {
            dbContext.Stages.Remove(item);
        }

        public IEnumerable<Stage> GetAll()
        {
            return dbContext.Stages;
        }

        public Stage GetById(object id)
        {
            return dbContext.Stages.Find((int)id);
        }

        public void Update(Stage item)
        {
            dbContext.EntryObj(item);
        }
    }
}
