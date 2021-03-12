using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARequestParticipantBridgeConfig : BaseConfig<EARequestParticipantBridge>
    {
        public EARequestParticipantBridgeConfig()
        {
            #region Relationship

            HasRequired(p => p.Participant)
                .WithMany(p => p.EARequestParticipantBridges)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaRequestParticipantBridges)
                .HasForeignKey(p => p.EARequestId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EaIndividualType)
                .WithMany()
                .HasForeignKey(p => p.EAIndividualTypeId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EaRelationshipType)
                .WithMany()
                .HasForeignKey(p => p.EARelationTypeId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EaSsnExemptType)
                .WithMany()
                .HasForeignKey(p => p.SSNExemptTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EARequestParticipantBridge");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EARequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EAIndividualTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EARelationTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsIncluded)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SSNAppliedDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.SSNExemptTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();

            #endregion
        }
    }
}
