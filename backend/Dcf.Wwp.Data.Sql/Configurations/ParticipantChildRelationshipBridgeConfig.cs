using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ParticipantChildRelationshipBridgeConfig : BaseCommonModelConfig<ParticipantChildRelationshipBridge>
    {
        public ParticipantChildRelationshipBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.ParticipantChildRelationshipBridges)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.Child)
                .WithMany(p => p.ParticipantChildRelationshipBridges)
                .HasForeignKey(p => p.ChildId);

            HasOptional(p => p.Relationship)
                .WithMany(p => p.ParticipantChildRelationshipBridges)
                .HasForeignKey(p => p.RelationshipId);

            #endregion

            #region Properties

            ToTable("ParticipantChildRelationshipBridge");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChildId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.RelationshipId)
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
                .IsOptional();

            #endregion
        }
    }
}
