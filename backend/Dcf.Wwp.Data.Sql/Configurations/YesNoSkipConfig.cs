using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class YesNoSkipConfig : EntityTypeConfiguration<YesNoSkipLookup>
    {
        public YesNoSkipConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("YesNoSkipLookup");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            #endregion
        }
    }
}
