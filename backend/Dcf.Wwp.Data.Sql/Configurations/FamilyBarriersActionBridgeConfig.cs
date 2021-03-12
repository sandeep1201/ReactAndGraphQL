using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FamilyBarriersActionBridgeConfig : BaseConfig<FamilyBarriersActionBridge>
    {
        public FamilyBarriersActionBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.FamilyBarriersAssessmentSection)
                .WithMany()
                .HasForeignKey(p => p.FamilyBarriersAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("FamilyBarriersActionBridge");

            Property(p => p.FamilyBarriersAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ActionNeededId)
                .HasColumnType("int")
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
