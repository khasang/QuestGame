using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class UserProfileDTO
    {
        public string UserId { get; set; }
        public string avatarUrl { get; set; }
        public int Rating { get; set; }
        public int CountCompliteQuests { get; set; }
        public DateTime? InviteDate { get; set; }
    }
}
