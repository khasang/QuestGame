using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        IDbSet<ApplicationUser> Users { get; }
        IDbSet<IdentityRole> Roles { get; }

        void Save();
    }
}
