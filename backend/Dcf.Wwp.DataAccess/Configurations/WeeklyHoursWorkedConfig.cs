using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WeeklyHoursWorkedConfig : BaseConfig<WeeklyHoursWorked>
    {
        public WeeklyHoursWorkedConfig()
        {
            #region Relationships

            HasRequired(p => p.EmploymentInformation)
                .WithMany(p => p.WeeklyHoursWorked)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

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
