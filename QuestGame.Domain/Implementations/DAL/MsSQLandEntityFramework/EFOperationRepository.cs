using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;

namespace QuestGame.Domain.Implementations
{
    class EFOperationRepository : IOperationRepository
    {
        private IDBContext db;

        public EFOperationRepository(IDBContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Operation item)
        {
            db.Operations.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetByID((int)id));
        }

        public void Delete(Operation item)
        {
            db.Operations.Remove(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Operation> GetAll()
        {
            return db.Operations;
        }

        public Operation GetByID(object id)
        {
            return db.Operations.Find((int)id);
        }

        public void Save()
        {
            db.Save();
        }

        public void Update(Operation item)
        {
            //db.Entry<Operation>(item);
        }
    }
}
