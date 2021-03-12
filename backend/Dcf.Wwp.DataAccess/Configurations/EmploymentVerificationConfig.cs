using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmploymentVerificationConfig : BaseConfig<EmploymentVerification>
    {
        public EmploymentVerificationConfig()
        {
            #region Relationship

            HasRequired(p => p.EmploymentInformation)
                .WithMany(p => p.EmploymentVerifications)
                .HasForeignKey(p => p.EmploymentInformationId);

            #endregion

            #region Properties

            ToTable("EmploymentVerification");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsVerified)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.NumberOfDaysAtVerification)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
