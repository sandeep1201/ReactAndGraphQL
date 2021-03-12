﻿using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FamilyBarriersAssessmentSectionConfig : BaseCommonModelConfig<FamilyBarriersAssessmentSection>
    {
        public FamilyBarriersAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments)
                .WithOptional(p => p.FamilyBarriersAssessmentSection)
                .HasForeignKey(p => p.FamilyBarriersAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("FamilyBarriersAssessmentSection");

            Property(p => p.ReviewCompleted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ActionDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
