using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAIPVConfig : BaseConfig<EAIPV>
    {
        public EAIPVConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.EaIpvs)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId);

            HasRequired(p => p.Occurrence)
                .WithMany()
                .HasForeignKey(p => p.OccurrenceId);

            HasRequired(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId);

            HasRequired(p => p.County)
                .WithMany()
                .HasForeignKey(p => p.CountyId);

            HasOptional(p => p.EaAlternateMailingAddress)
                .WithMany(p => p.EaIpvs)
                .HasForeignKey(p => p.MailingAddressId);

            HasMany(p => p.EaIpvReasonBridges)
                .WithRequired(p => p.EaIpv)
                .HasForeignKey(p => p.IPVId);

            #endregion

            #region Properties

            ToTable("EAIPV");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.DeterminationDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.IPVNumber)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OccurrenceId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.MailingAddressId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StatusId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PenaltyStartDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.PenaltyEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.CountyId)
                .HasColumnType("int")
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
                .IsRequired();

            #endregion
        }
    }
}
