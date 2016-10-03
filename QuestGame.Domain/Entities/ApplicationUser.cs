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

        public string NickName { get; set; }

        //public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        /// <summary>
        /// Квесты, созданные пользователем
        /// </summary>
        public virtual ICollection<Quest> Quests { get; set; }

        public ApplicationUser()
        {
            Quests = new List<Quest>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте настраиваемые утверждения пользователя

            //userIdentity.AddClaim(new Claim(ClaimTypes.Name, this.NickName));

            return userIdentity;
        }
    }    
}
