using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DBInitializers
{
    public interface IInitialization
    {
        void Initialization(ApplicationDbContext dbContext);
    }
}
