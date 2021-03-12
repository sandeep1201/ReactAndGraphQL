using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TimeLimitSummaryConfig : BaseCommonModelConfig<TimeLimitSummary>
    {
        public TimeLimitSummaryConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.TimeLimitSummaries)
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("TimeLimitSummary");

            Property(p => p.FederalUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.FederalMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StateUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StateMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CSJUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CSJMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.W2TUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.W2TMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TMPUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TNPUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TempUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TempMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CMCUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CMCMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OPCUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OPCMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OtherUsed)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OtherMax)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OTF)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Tribal)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TJB)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JOBS)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NO24)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.FactDetails)
                .HasColumnType("varchar")
                .HasMaxLength(4000)
                .IsOptional();

            Property(p => p.CSJExtensionDue)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.W2TExtensionDue)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.TempExtensionDue)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.StateExtensionDue)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
