using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class SocialUserDTO
    {
        [Required]
        public string SocialId { get; set; }

        public string NickName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string AvatarUrl { get; set; }

        [Required]
        public string Provider { get; set; }
    }
}
