using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;

namespace QuestGame.Domain.Implementations
{
    class EFContentStageRepository : QuestGame.Domain.Interfaces.IContentStageRepository
    {
        private IDBContext db;

        public EFContentStageRepository(IDBContext dbContext)
        {
            db = dbContext;
        }

        public void Add(ContentStage item)
        {
            db.StageContents.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetByID((int)id));
        }

        public void Delete(ContentStage item)
        {
            db.StageContents.Remove(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentStage> GetAll()
        {
            return db.StageContents;
        }

        public ContentStage GetByID(object id)
        {
            return db.StageContents.Find((int)id);
        }

        public void Update(ContentStage item)
        {
           // db.Entry<ContentStage>(item);
        }
    }
}
