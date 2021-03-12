using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class HolidayLookUpConfig : BaseConfig<HolidayLookUp>
    {
        public HolidayLookUpConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("HolidayLookUp");

            Property(p => p.Year)
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsOptional();

            Property(p => p.Date)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsCARESHoliday)
                .HasColumnName("CARESHoliday")
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsFederalHoliday)
                .HasColumnName("FederalHoliday")
                .HasColumnType("bit")
                .IsRequired();

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
