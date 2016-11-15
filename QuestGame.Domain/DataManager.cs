using Microsoft.AspNet.Identity;
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

        RoleManager<IdentityRole> roleManager;
        ApplicationUserManager userManager;
        IQuestRepository questRepository;
        IStageRepository stageRepository;
        IMotionRepository motionRrepository;
        IUserRepository userRepository;        
        IRoleRepository roleRepository;
        IImageRepository imageRepository;

        public DataManager(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQuestRepository Quests => questRepository ?? (questRepository = new EFQuestRepository(dbContext));

        public IStageRepository Stages => stageRepository ?? (stageRepository = new EFStageRepository(dbContext));

        public IMotionRepository Motions => motionRrepository ?? (motionRrepository = new EFMotionRepository(dbContext));

        public IImageRepository Images => imageRepository ?? (imageRepository = new EFImageRepository(dbContext));

        public IUserRepository Users => userRepository ?? (userRepository = new EFUserRepository(dbContext));

        public IRoleRepository Roles => roleRepository ?? (roleRepository = new EFRoleRepository(dbContext));

        public ApplicationUserManager UserManager => userManager ??
                                                     (userManager = new ApplicationUserManager(new UserStore<ApplicationUser>((DbContext) dbContext)));

        public RoleManager<IdentityRole> RoleManager => roleManager ??
                                                        (roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>((DbContext) dbContext)));

        public void Save()
        {
            dbContext.SaveChanges();
        }

        private bool disposed = false;

        public void Dispose(bool disposing)
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
