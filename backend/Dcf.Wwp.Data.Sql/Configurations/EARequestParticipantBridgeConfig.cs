using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EARequestParticipantBridgeConfig : BaseCommonModelConfig<EARequestParticipantBridge>
    {
        public EARequestParticipantBridgeConfig()
        {
            #region Relationship

            HasRequired(p => p.Participant)
                .WithMany(p => p.EaRequestParticipantBridges)
                .HasForeignKey(p => p.ParticipantId);

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
