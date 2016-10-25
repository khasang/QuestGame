using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class UserProfile
    {
        public string avatarUrl { get; set; }
        public int Rating { get; set; }
        public int CountCompliteQuests { get; set; }
        public DateTime? InviteDate { get; set; }

        public string UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
