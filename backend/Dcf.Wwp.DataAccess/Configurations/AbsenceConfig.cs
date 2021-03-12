using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class AbsenceConfig: BaseConfig<Absence>
    {
        public AbsenceConfig()
        {
            #region Relationships
            HasRequired(p => p.EmploymentInformation)
                .WithMany(p => p.Absences)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

            #endregion


            #region Properties

            ToTable("Absence");
            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AbsenceReasonId)
                .HasColumnType("int")
                .IsOptional();
            Property(p => p.BeginDate)
                .HasColumnType("datetime")
                .IsOptional();
            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();
            Property(p => p.Details)
                .HasColumnType("varchar")
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
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }

    }
}
