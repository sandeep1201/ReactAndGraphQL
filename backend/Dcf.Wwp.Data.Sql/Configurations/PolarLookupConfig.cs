using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PolarLookupConfig : EntityTypeConfiguration<PolarLookup>
    {
        // Note: does not inherit from BaseConfig, because
        // the entity itself doesn't inherit from Base..

        public PolarLookupConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("PolarLookup");

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
