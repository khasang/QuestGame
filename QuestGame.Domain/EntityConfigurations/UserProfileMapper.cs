using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.EntityConfigurations
{
    public class UserProfileMapper : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMapper()
        {
            this.HasKey(x => x.UserId);
            //this.HasRequired(x => x.User).WithOptional(x => x.UserProfile);
        }
    }
}
