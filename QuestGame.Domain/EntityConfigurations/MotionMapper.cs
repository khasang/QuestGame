using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.EntityConfigurations
{
    public class MotionMapper : EntityTypeConfiguration<Motion>
    {
        public MotionMapper()
        {
            this.ToTable("Motion");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Id).IsRequired();
            this.Property(x => x.Description).IsRequired();

            this.HasRequired(x => x.OwnerStage)
                .WithMany(x => x.Motions)
                .HasForeignKey(x => x.OwnerStageId);
        }
    }
}
