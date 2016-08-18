using System.Collections.Generic;

namespace QuestGame.Domain.Interfaces
{
    public interface ICommonRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void Delete(object id);
    }
}
