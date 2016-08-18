using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IDataManager
    {
        IQuestRepository Quests { get; }
        IStageRepository Stages { get; }
        IMotionRepository Motions { get; }

        void Save();
    }
}
