using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IQuestRepository : IRepositoryQG<Quest>
    {
        IEnumerable<Quest> GetByIdentificator(string identificator);
    }
}
