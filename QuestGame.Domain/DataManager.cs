using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Implementations;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain
{
    public class DataManager : IDataManager
    {
        ApplicationDbContext dbContext;

        IQuestRepository questRepository;
        IStageRepository stageRepository;
        IMotionRepository motionRrepository;

        public DataManager()
        {
            dbContext = new ApplicationDbContext();
        }

        public IQuestRepository Quests
        {
            get
            {
                if(questRepository == null)
                {
                    questRepository = new EFQuestRepository(dbContext);
                }
                return questRepository;
            }
        }

        public IStageRepository Stages
        {
            get
            {
                if (stageRepository == null)
                {
                    stageRepository = new EFStageRepository(dbContext);
                }
                return stageRepository;
            }
        }

        public IMotionRepository Motions
        {
            get
            {
                if (motionRrepository == null)
                {
                    motionRrepository = new EFMotionRepository(dbContext);
                }
                return motionRrepository;
            }
        }

        public IDbSet<ApplicationUser> Users
        {
            get
            {
                return dbContext.GetUsers();
            }
        }

        public IDbSet<IdentityRole> Roles
        {
            get
            {
                return dbContext.GetRoles();
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
