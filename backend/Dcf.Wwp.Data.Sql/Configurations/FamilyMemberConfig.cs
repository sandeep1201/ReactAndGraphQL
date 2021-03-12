using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FamilyMemberConfig : BaseConfig<FamilyMember>
    {
        public FamilyMemberConfig()
        {
            #region Relationships

            HasOptional(p => p.FamilyBarriersSection)
                .WithMany(p => p.FamilyMembers)
                .HasForeignKey(p => p.FamilyBarriersSectionId);

            HasOptional(p => p.Relationship)
                .WithMany(p => p.FamilyMembers)
                .HasForeignKey(p => p.RelationshipId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.FamilyMembers)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("FamilyMember");

            Property(p => p.FamilyBarriersSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.RelationshipId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

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
