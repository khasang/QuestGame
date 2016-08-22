using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IRepositoryQG<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetAll();
        T GetByID( object id);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void Delete( object id);
    }
}
