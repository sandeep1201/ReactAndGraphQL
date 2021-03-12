using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierSectionConfig : BaseCommonModelConfig<BarrierSection>
    {
        public BarrierSectionConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany() // .WithMany(p => p.BarrierSections) //see comments in Participart.cs nav props
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.BarrierDetails)
                .WithOptional(p => p.BarrierSection)
                .HasForeignKey(p => p.BarrierSectionId);

            HasOptional(p => p.YesNoRefusedIsPhysicalHealthHardToManage)
                .WithMany()
                .HasForeignKey(p => p.IsPhysicalHealthHardToManageId);

            HasOptional(p => p.YesNoRefusedIsPhysicalHealthHardHardToParticipate)
                .WithMany()
                .HasForeignKey(p => p.IsPhysicalHealthHardToParticipateId);

            HasOptional(p => p.YesNoRefusedIsPhysicalHealthTakeMedication)
                .WithMany()
                .HasForeignKey(p => p.IsPhysicalHealthTakeMedicationId);

            HasOptional(p => p.YesNoRefusedIsMentalHealthDiagnosed)
                .WithMany()
                .HasForeignKey(p => p.IsMentalHealthHardDiagnosedId);

            HasOptional(p => p.YesNoRefusedIsMentalHealthHardToManage)
                .WithMany()
                .HasForeignKey(p => p.IsMentalHealthHardToManageId);

            HasOptional(p => p.YesNoRefusedIsMentalHealthHardToParticipate)
                .WithMany()
                .HasForeignKey(p => p.IsMentalHealthHardToParticipateId);

            HasOptional(p => p.YesNoRefusedIsMentalHealthTakeMedication)
                .WithMany()
                .HasForeignKey(p => p.IsMentalHealthTakeMedicationId);

            HasOptional(p => p.YesNoRefusedIsAODAHardToManage)
                .WithMany()
                .HasForeignKey(p => p.IsAODAHardToManageId);

            HasOptional(p => p.YesNoRefusedIsAODAHardToParticipate)
                .WithMany()
                .HasForeignKey(p => p.IsAODAHardToParticipateId);

            HasOptional(p => p.YesNoRefusedIsAODATakeTreatment)
                .WithMany()
                .HasForeignKey(p => p.IsAODATakeTreatmentId);

            HasOptional(p => p.YesNoRefusedIsLearningDisabilityDiagnosed)
                .WithMany()
                .HasForeignKey(p => p.IsLearningDisabilityDiagnosedId);

            HasOptional(p => p.YesNoRefusedIsLearningDisabilityHardToManage)
                .WithMany()
                .HasForeignKey(p => p.IsLearningDisabilityHardToManageId);

            HasOptional(p => p.YesNoRefusedIsLearningDisabilityHardToParticipate)
                .WithMany()
                .HasForeignKey(p => p.IsLearningDisabilityHardToParticipateId);

            HasOptional(p => p.YesNoRefused6)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceHurtingYouOrOthersId);

            HasOptional(p => p.YesNoRefused5)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId);

            HasOptional(p => p.YesNoRefused10)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolencePartnerControlledMoneyId);

            HasOptional(p => p.YesNoRefused11)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceReceivedServicesOrLivedInShelterId);

            HasOptional(p => p.YesNoRefused4)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceEmotionallyOrVerballyAbusingId);

            HasOptional(p => p.YesNoRefused3)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceCallingHarassingStalkingAtWorkId);

            HasOptional(p => p.YesNoRefused8)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceMakingItDifficultToWorkId);

            HasOptional(p => p.YesNoRefused9)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId);

            HasOptional(p => p.YesNoRefused7)
                .WithMany()
                .HasForeignKey(p => p.IsDomesticViolenceInvolvedInCourtsId);

            #endregion

            #region Properties

            ToTable("BarrierSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsPhysicalHealthHardToManageDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsPhysicalHealthHardToManageId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsPhysicalHealthHardToParticipateDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsPhysicalHealthHardToParticipateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsPhysicalHealthTakeMedicationDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsPhysicalHealthTakeMedicationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsMentalHealthHardDiagnosedDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsMentalHealthHardDiagnosedId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsMentalHealthHardToManageDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsMentalHealthHardToManageId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsMentalHealthHardToParticipateDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsMentalHealthHardToParticipateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsMentalHealthTakeMedicationDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsMentalHealthTakeMedicationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsAODAHardToManageDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsAODAHardToManageId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsAODAHardToParticipateDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsAODAHardToParticipateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsAODATakeTreatmentDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsAODATakeTreatmentId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsLearningDisabilityDiagnosedDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsLearningDisabilityDiagnosedId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsLearningDisabilityHardToManageDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsLearningDisabilityHardToManageId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsLearningDisabilityHardToParticipateDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsLearningDisabilityHardToParticipateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceHurtingYouOrOthersId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceHurtingYouOrOthersDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolencePartnerControlledMoneyId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolencePartnerControlledMoneyDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceReceivedServicesOrLivedInShelterId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceReceivedServicesOrLivedInShelterDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceEmotionallyOrVerballyAbusingId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceEmotionallyOrVerballyAbusingDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceCallingHarassingStalkingAtWorkId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceCallingHarassingStalkingAtWorkDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceMakingItDifficultToWorkId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceMakingItDifficultToWorkDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDomesticViolenceInvolvedInCourtsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDomesticViolenceInvolvedInCourtsDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
