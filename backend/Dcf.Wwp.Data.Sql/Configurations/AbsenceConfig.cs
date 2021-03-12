using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AbsenceConfig : BaseCommonModelConfig<Absence>
    {
        public AbsenceConfig()
        {
            #region Relationships

            HasOptional(p => p.AbsenceReason)
                .WithMany()
                .HasForeignKey(d => d.AbsenceReasonId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.EmploymentInformation)
                .WithMany(p => p.Absences)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Absence");

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.AbsenceReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

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
