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
                Birthday  = DateTime.Now - new TimeSpan(10000, 0, 0, 0, 0),
                Sex = true,
                User = owner
            };

            var owner2 = dbContext.Users.FirstOrDefault(x => x.Email == "Dane@gmail.com");

            var profile2 = new UserProfile
            {
                Birthday = DateTime.Now - new TimeSpan(10000, 0, 0, 150, 50),
                Sex = true,
                User = owner2
            };

            dbContext.UserProfiles.Add(profile);
            dbContext.UserProfiles.Add(profile2);
            dbContext.SaveChanges();
        }
    }
}
