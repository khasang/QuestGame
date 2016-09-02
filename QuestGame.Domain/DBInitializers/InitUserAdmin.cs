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
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            var user = new ApplicationUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                AddDate = DateTime.Now
            };

            IdentityRole role = new IdentityRole("admin");

            userManager.Create(user, "qwerty");
            roleManager.Create(role);
            userManager.AddToRole(user.Id, role.Name);

            var user1 = new ApplicationUser()
            {
                UserName = "kloder3@gmail.com",
                Email = "kloder3@gmail.com",
                EmailConfirmed = true,
                AddDate = DateTime.Now
            };

            role = new IdentityRole("admin");

            userManager.Create(user1, "qwerty");
            roleManager.Create(role);
            userManager.AddToRole(user1.Id, role.Name);
        }
    }
}
