using QuestGame.Domain.DBInitializers;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DBInitializers
{
    public class UserInit : IInitialization
    {
        public void Initialization(ApplicationDbContext context)
        {
            var user = new ApplicationUser();
            user.Email = "admin@admin.com";

            context.Users.Add(user);
        }
    }
}
