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

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true
            };

            IdentityRole role = new IdentityRole("admin");

            userManager.Create(user, "qwerty");
            roleManager.Create(role);
            userManager.AddToRole(user.Id, role.Name);
        }
    }
}
