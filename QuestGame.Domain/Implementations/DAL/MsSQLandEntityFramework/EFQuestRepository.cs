using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;

namespace QuestGame.Domain.Implementations
{
    class EFQuestRepository : QuestGame.Domain.Interfaces.IQuestRepository
    {
        private ApplicationDbContext db;

        public EFQuestRepository(ApplicationDbContext dbContext )
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

        public void Update(Quest item)
        {
            db.Entry<Quest>(item);
        }
    }
}
