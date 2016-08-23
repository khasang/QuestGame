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
    public class QuestMapper : EntityTypeConfiguration<Quest>
    {
        public QuestMapper()
        {
            this.ToTable("Quest");

            this.Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.Id).IsRequired();
            this.Property(m => m.Title).IsRequired();
            this.Property(m => m.Date).IsRequired();

            this.HasMany(x => x.Stages)
                .WithRequired(x => x.Quest)
                .HasForeignKey(x => x.QuestId);
        }
    }
}
