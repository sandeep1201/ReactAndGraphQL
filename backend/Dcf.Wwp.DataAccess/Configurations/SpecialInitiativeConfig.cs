using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class SpecialInitiativeConfig : EntityTypeConfiguration<SpecialInitiative>
    {
        public SpecialInitiativeConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("SpecialInitiative");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParameterName)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsOptional();

            Property(p => p.ParameterValue)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
