using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class CountyAndTribeConfig : BaseConfig<CountyAndTribe>
    {
        public CountyAndTribeConfig()
        {
            #region Relationships

            HasMany(p => p.Offices)
                .WithOptional(p => p.CountyAndTribe)
                .HasForeignKey(p => p.CountyandTribeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("CountyAndTribe");

            Property(p => p.CountyNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.CountyName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.IsCounty)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.AgencyName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.LocationNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
