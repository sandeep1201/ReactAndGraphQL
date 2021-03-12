using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FormalAssessmentConfig : BaseConfig<FormalAssessment>
    {
        public FormalAssessmentConfig()
        {
            #region Relationships

            HasOptional(p => p.BarrierDetail)
                .WithMany(p => p.FormalAssessments)
                .HasForeignKey(p => p.BarrierDetailsId);

            HasOptional(p => p.Symptom)
                .WithMany(p => p.FormalAssessments)
                .HasForeignKey(p => p.SymptomId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.FormalAssessments)
                .HasForeignKey(p => p.DeleteReasonId);

            HasOptional(p => p.IntervalType)
                .WithMany(p => p.FormalAssessments)
                .HasForeignKey(p => p.HoursParticipantCanParticipateIntervalId);

            #endregion

            #region Properties

            ToTable("FormalAssessment");

            Property(p => p.BarrierDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ReferralDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.ReferralDeclined)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ReferralDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.AssessmentDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.AssessmentNotCompleted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AssessmentDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.SymptomId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ReassessmentRecommendedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsRecommendedDateNotNeeded)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SymptomDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.AssessmentProviderContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HoursParticipantCanParticipate)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HoursParticipantCanParticipateDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.HoursParticipantCanParticipateIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

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
