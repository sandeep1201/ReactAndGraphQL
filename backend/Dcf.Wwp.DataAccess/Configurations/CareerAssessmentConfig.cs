using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class CareerAssessmentConfig : BaseConfig<CareerAssessment>
    {
        public CareerAssessmentConfig()
        {
            #region Relationsships

            HasRequired(p => p.Contact)
                .WithMany()
                .HasForeignKey(p => p.CareerAssessmentContactId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Participant)
                .WithMany(p => p.CareerAssessments)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.CareerAssessmentElementBridges)
                .WithRequired(p => p.CareerAssessment)
                .HasForeignKey(p => p.CareerAssessmentId)
                .WillCascadeOnDelete(false);
            #endregion

            #region Properties

            ToTable("CareerAssessment");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();
            Property(p => p.CompletionDate)
                .HasColumnType("datetime")
                .IsRequired();
            Property(p => p.AssessmentProvider)
                .HasColumnType("varchar")
                .IsRequired();
            Property(p => p.AssessmentToolUsed)
                .HasColumnType("varchar")
                .IsRequired();
            Property(p => p.AssessmentResults)
                .HasColumnType("varchar")
                .IsRequired();
            Property(p => p.CareerAssessmentContactId)
                .HasColumnType("int")
                .IsOptional();
            Property(p => p.RelatedOccupation)
                .HasColumnType("varchar")
                .IsOptional();
            Property(p => p.AssessmentResultAppliedToEP)
                .HasColumnType("varchar")
                .IsOptional();
            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();
            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsRequired();
            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();
            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
