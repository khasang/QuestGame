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
        public DateTime Birthday { get; set; }
        public bool Sex { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
