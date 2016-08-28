using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;

namespace QuestGame.Domain.Implementations
{
    class EFContentQuestRepository : QuestGame.Domain.Interfaces.IContentQuestRepository
    {
        private IDBContext db;

        public EFContentQuestRepository(IDBContext dbContext)
        {
            db = dbContext;
        }

        public void Add(ContentQuest item)
        {
            db.QuestContents.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetByID((int)id));
        }

        public void Delete(ContentQuest item)
        {
            db.QuestContents.Remove(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentQuest> GetAll()
        {
            return db.QuestContents;
        }

        public ContentQuest GetByID(object id)
        {
            return db.QuestContents.Find((int)id);
        }

        public void Update(ContentQuest item)
        {
           // db.Entry<ContentQuest>(item);
        }
    }
}
