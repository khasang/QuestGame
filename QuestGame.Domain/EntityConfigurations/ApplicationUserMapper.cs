using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.EntityConfigurations
{
    public class ApplicationUserMapper : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMapper()
        {
            this.HasMany(x => x.Quests)
                .WithRequired(x => x.Owner)
                .HasForeignKey(x => x.OwnerId);
        }
    }
}
