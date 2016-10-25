using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DTO
{
    public class ResetPasswordDTO
    {
        public string Id { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
