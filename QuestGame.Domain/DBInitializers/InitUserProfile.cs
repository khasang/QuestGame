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

            dbContext.UserProfiles.Add(profile);
            dbContext.SaveChanges();
        }
    }
}
