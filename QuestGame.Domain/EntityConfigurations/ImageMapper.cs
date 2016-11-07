using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;

namespace QuestGame.Domain.EntityConfigurations
{
    public class ImageMapper : EntityTypeConfiguration<Image>
    {
        public ImageMapper()
        {
            this.ToTable("Images");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Id).IsRequired();
            this.Property(x => x.Ext).IsRequired();
        }
    }
}
