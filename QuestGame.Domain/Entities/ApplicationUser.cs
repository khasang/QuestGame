using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.AddDate = DateTime.Now;
        }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public string Contry { get; set; }

        public int Rating { get; set; }

        public int CountQuestsComplite { get; set; }

        public string Token { get; set; }

        public DateTime AddDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте настраиваемые утверждения пользователя

            userIdentity.AddClaim(new Claim(ClaimTypes.Name, this.Name + " " + this.LastName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Country, this.Contry));
            userIdentity.AddClaim(new Claim("Rating", this.Rating.ToString()));
            userIdentity.AddClaim(new Claim("CountQuestsComplite", this.CountQuestsComplite.ToString()));

            return userIdentity;
        }
    }
}
