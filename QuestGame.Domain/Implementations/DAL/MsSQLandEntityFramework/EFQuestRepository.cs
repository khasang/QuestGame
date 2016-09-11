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
    class EFQuestRepository : IQuestRepository
    {
        private IDBContext db;

        public EFQuestRepository(IDBContext dbContext )
        {
            this.db = dbContext;
        }

        public void Add(Quest item)
        {
            this.db.Quests.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetByID((int)id));
        }

        public void Delete(Quest item)
        {
            this.db.Quests.Remove(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quest> GetAll()
        {
            return db.Quests;
        }

        public Quest GetByID(object id)
        {
            return db.Quests.Find( (int)id );
        }

        public IEnumerable<Quest> GetByIdentificator(string identificator)
        {
            var r = db.Quests.Where(q => q.User.Identificator == identificator);
            return r;
        }

        public void Save()
        {
            db.Save();
        }

        public void Update(Quest item)
        {
            db.Attach(item);
           //db.Entry<Quest>(item).State = EntityState.Modified;
        }
    }
}
