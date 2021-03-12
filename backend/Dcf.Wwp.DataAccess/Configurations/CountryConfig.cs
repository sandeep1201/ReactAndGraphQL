using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class CountryConfig : BaseConfig<Country>
    {
        public CountryConfig()
        {
            #region Relationships

            HasMany(p => p.States)
                .WithOptional(p => p.Country)
                .HasForeignKey(p => p.CountryId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Cities)
                .WithOptional(p => p.Country)
                .HasForeignKey(p => p.CountryId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Country");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsRequired();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.IsNonStandard)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
