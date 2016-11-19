using QuestGame.Common.Helpers;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DBInitializers
{
    public class InitUserProfile : IInitialization
    {
        public void Initialization(ApplicationDbContext dbContext)
        {
            var owner = dbContext.Users.FirstOrDefault(x => x.Email == "admin@admin.com");

            var profile = new UserProfile
            {
                InviteDate = DateTime.Now,
                CountCompliteQuests = 5,
                User = owner,
                Avatar = new Image
                {
                    Name = @"http://localhost:9243/Content/Images/NoImageAvailable.png",
                    Prefix = string.Empty,
                }
            };

            dbContext.UserProfiles.Add(profile);
            dbContext.SaveChanges();
        }
    }
}
