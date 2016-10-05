using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public UserProfileDTO UserProfile { get; set; }

        public IEnumerable<QuestDTO> Quests { get; set; }
    }
}
