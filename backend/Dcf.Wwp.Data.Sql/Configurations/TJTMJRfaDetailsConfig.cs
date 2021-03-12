using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TJTMJRfaDetailsConfig : BaseCommonModelConfig<TJTMJRfaDetail>
    {
        public TJTMJRfaDetailsConfig()
        {
            #region Relationships

            HasOptional(p => p.RequestForAssistance)
                .WithMany()
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasOptional(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.ContractorId);

            #endregion

            #region Properties

            ToTable("TJTMJRfaDetails"); // NOTE: this table was pluralized unlike the rest...

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContractorId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ApplicationDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.ApplicationDueDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsUnder18)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HouseholdSizeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HouseholdIncome)
                .HasColumnType("money")
                .IsOptional();

            Property(p => p.LastEmploymentDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.HasWorkedLessThan16Hours)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsEligibleForUnemployment)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsReceivingW2Benefits)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsCitizen)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasWorked1040Hours)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAppCompleteAndSigned)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasEligibilityBeenVerified)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsBenefitFromSubsidizedJob)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.BenefitFromSubsidizedJobDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsEligible)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.PopulationTypeDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.HasNeverEmployed)
                .HasColumnType("bit")
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
