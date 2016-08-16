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
    public class StageMapper : EntityTypeConfiguration<Stage>
    {
        public StageMapper()
        {
            this.ToTable("Stage");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Id).IsRequired();
            this.Property(x => x.Title).IsRequired();
            this.Property(x => x.Body).IsRequired();

            this.HasMany(x => x.Motions)
                .WithRequired(x => x.OwnerStage)
                .HasForeignKey(x => x.OwnerStageId);

            this.HasRequired(x => x.Quest)
                .WithMany(x => x.Stages);
        }
    }
}
