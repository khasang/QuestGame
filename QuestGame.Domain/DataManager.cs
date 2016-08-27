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
        IApplicationDbContext dbContext;

        ApplicationUserManager userManager;
        IQuestRepository questRepository;
        IStageRepository stageRepository;
        IMotionRepository motionRrepository;
        IUserRepository userRepository;
        
        IRoleRepository roleRepository;

        public DataManager(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new EFUserRepository(dbContext);
                }
                return userRepository;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new EFRoleRepository(dbContext);
                }
                return roleRepository;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if (userManager == null)
                {
                    userManager = new ApplicationUserManager(new UserStore<ApplicationUser>((DbContext)(dbContext)));
                }
                return userManager;
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
