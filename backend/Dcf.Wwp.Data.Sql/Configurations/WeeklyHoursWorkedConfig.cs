using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WeeklyHoursWorkedConfig : BaseCommonModelConfig<WeeklyHoursWorked>
    {
        public WeeklyHoursWorkedConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("WeeklyHoursWorked");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.StartDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.Hours)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.TotalSubsidyAmount)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.TotalWorkSiteAmount)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(380)
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
                .IsRequired();

            #endregion
        }
    }
}
