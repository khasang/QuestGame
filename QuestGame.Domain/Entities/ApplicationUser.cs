using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
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
        //private Guid identificator;

        public ApplicationUser()
        {
            //this.AddDate = DateTime.Now;
            //this.identificator = Guid.NewGuid();
            this.Quests = new List<Quest>();
            this.QuestsRoutes = new List<QuestRoute>();
        }

        public string Identificator { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime? Bithday { get; set; }

        public string Avatar { get; set; }

        public string Country { get; set; }

        public int Rating { get; set; }

        public int CountQuestsComplite { get; set; }

        public string Token { get; set; }

        public DateTime AddDate { get; set; }


        [JsonIgnore]
        public virtual ICollection<Quest> Quests { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestRoute> QuestsRoutes { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте настраиваемые утверждения пользователя

            userIdentity.AddClaim(new Claim(ClaimTypes.Name, this.Name + " " + this.LastName));
            userIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth, this.Bithday.ToString()));
            userIdentity.AddClaim(new Claim("Avatar", this.Avatar));
            userIdentity.AddClaim(new Claim("Country", this.Country));
            userIdentity.AddClaim(new Claim("Rating", this.Rating.ToString()));
            userIdentity.AddClaim(new Claim("AddDate", this.AddDate.ToString()));
            userIdentity.AddClaim(new Claim("Identificator", this.Identificator));

            return userIdentity;
        }
    }
}
