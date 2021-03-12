using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class LegalIssuesSectionConfig : BaseCommonModelConfig<LegalIssuesSection>
    {
        public LegalIssuesSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.LegalIssuesSections)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.LegalIssuesSections)
                .HasForeignKey(p => p.CommunitySupervisonContactId);

            HasMany(p => p.Convictions)
                .WithOptional(p => p.LegalIssuesSection)
                .HasForeignKey(p => p.LegalSectionId);

            HasMany(p => p.CourtDates)
                .WithOptional(p => p.LegalIssuesSection)
                .HasForeignKey(p => p.LegalSectionId);

            HasMany(p => p.PendingCharges)
                .WithOptional(p => p.LegalIssuesSection)
                .HasForeignKey(p => p.LegalSectionId);

            #endregion

            #region Properties

            ToTable("LegalIssuesSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsConvictedOfCrime)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsUnderCommunitySupervision)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CommunitySupervisonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.HasPendingCharges)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasFamilyLegalIssues)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.FamilyLegalIssueNotes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.HasCourtDates)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ActionNeededDetails)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.OrderedToPayChildSupport)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.MonthlyAmount)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.IsUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.OweAnyChildSupportBack)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ChildSupportDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.CommunitySupervisonContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.HasRestrainingOrders)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.RestrainingOrderNotes)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.HasRestrainingOrderToPrevent)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.RestrainingOrderToPreventNotes)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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
