using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;
using System.Data.Entity;
using QuestGame.Domain.Interfaces;

namespace QuestGame.Domain.Implementations
{
    class EFStageRepository : IStageRepository
    {
        private IDBContext db;

        public EFStageRepository(IDBContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Stage item)
        {
            db.Stages.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetByID((int)id));
        }

        public void Delete(Stage item)
        {
            db.Stages.Remove(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Stage> GetAll()
        {
            return db.Stages;
        }

        public Stage GetByID(object id)
        {
            return db.Stages.Find((int)id);
        }

        public void Save()
        {
            db.Save();
        }

        public void Update(Stage item)
        {
            //db.Entry(item).State = EntityState.Modified;
        }
    }
}
