using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialChildConfig : BaseConfig<NonCustodialChild>
    {
        public NonCustodialChildConfig()
        {
            #region Relationships

            HasRequired(p => p.NonCustodialCaretaker)
                .WithMany(p => p.NonCustodialChilds)
                .HasForeignKey(p => p.NonCustodialCaretakerId);

            HasOptional(p => p.ContactInterval)
                .WithMany(p => p.NonCustodialChilds)
                .HasForeignKey(p => p.ContactIntervalId);

            HasOptional(p => p.HasOtherAdultsYesNoUnknownLookup)
                .WithMany()
                .HasForeignKey(p => p.HasOtherAdultsYesNoUnknownLookupId);

            HasOptional(p => p.IsNeedOfServicesYesNoUnknownLookup)
                .WithMany()
                .HasForeignKey(p => p.IsNeedOfServicesYesNoUnknownLookupId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.NonCustodialChilds)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("NonCustodialChild");

            Property(p => p.NonCustodialCaretakerId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            Property(p => p.DateOfBirth)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.HasChildSupportOrder)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ChildSupportOrderDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.ContactIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactIntervalDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.HasOtherAdultsYesNoUnknownLookupId)
                .HasColumnName("HasOtherAdultsYesNoUnknownLookupId")
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OtherAdultsDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsRelationshipChangeRequested)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.RelationshipChangeRequestedDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsNeedOfServicesYesNoUnknownLookupId)
                .HasColumnName("IsNeedOfServicesYesNoUnknownLookupId")
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NeedOfServicesDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.HasNameOnChildBirthRecord)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ModifiedBy)
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
