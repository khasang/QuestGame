using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IQuestRepository : ICommonRepository<Quest>
    {
        Quest GetByTitle(string title);
        IEnumerable<Quest> GetByActive();
        IEnumerable<Quest> GetByUserName(string userName);
        void DeleteByTitle(string title);
    }
}
