using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.DBInitializers;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DBInitializers
{
    public class InitUserAdmin : IInitialization
    {
        public void Initialization(ApplicationDbContext dbContext)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            var user = new ApplicationUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true
            };

            roleManager.Create(new IdentityRole("admin"));
            //roleManager.Create(new IdentityRole("user"));

            userManager.Create(user, "qwerty");            
            userManager.AddToRole(user.Id, "admin");

            var user2 = new ApplicationUser()
            {
                UserName = "Dane@gmail.com",
                Email = "Dane@gmail.com",
                EmailConfirmed = true
            };

            roleManager.Create(new IdentityRole("user"));

            userManager.Create(user2, "qwerty2");
            userManager.AddToRole(user2.Id, "user");
        }
    }
}
