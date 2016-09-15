using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;

namespace QuestGame.Domain.Implementations
{
    public class EFMotionRepository : IMotionRepository
    {
        private IApplicationDbContext dbContext;

        public EFMotionRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Motion item)
        {
            dbContext.Motions.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetById((int)id));
        }

        public void Delete(Motion item)
        {
            dbContext.Motions.Remove(item);
        }

        public IEnumerable<Motion> GetAll()
        {
            return dbContext.Motions;
        }

        public Motion GetById(object id)
        {
            return dbContext.Motions.Find((int)id);
        }

        public void Update(Motion item)
        {
            dbContext.EntryObj(item);
        }

        public IEnumerable<Motion> GetMotionsByStageId(int stageId)
        {
            return dbContext.Motions.Where(c => c.OwnerStageId == stageId).ToList();   //
        }
    }
}
