using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierSection
    {
        #region Properties

        public int?      ParticipantId                                             { get; set; }
        public string    IsPhysicalHealthHardToManageDetails                       { get; set; }
        public int?      IsPhysicalHealthHardToManageId                            { get; set; }
        public string    IsPhysicalHealthHardToParticipateDetails                  { get; set; }
        public int?      IsPhysicalHealthHardToParticipateId                       { get; set; }
        public string    IsPhysicalHealthTakeMedicationDetails                     { get; set; }
        public int?      IsPhysicalHealthTakeMedicationId                          { get; set; }
        public string    IsMentalHealthHardDiagnosedDetails                        { get; set; }
        public int?      IsMentalHealthHardDiagnosedId                             { get; set; }
        public string    IsMentalHealthHardToManageDetails                         { get; set; }
        public int?      IsMentalHealthHardToManageId                              { get; set; }
        public string    IsMentalHealthHardToParticipateDetails                    { get; set; }
        public int?      IsMentalHealthHardToParticipateId                         { get; set; }
        public string    IsMentalHealthTakeMedicationDetails                       { get; set; }
        public int?      IsMentalHealthTakeMedicationId                            { get; set; }
        public string    IsAODAHardToManageDetails                                 { get; set; }
        public int?      IsAODAHardToManageId                                      { get; set; }
        public string    IsAODAHardToParticipateDetails                            { get; set; }
        public int?      IsAODAHardToParticipateId                                 { get; set; }
        public string    IsAODATakeTreatmentDetails                                { get; set; }
        public int?      IsAODATakeTreatmentId                                     { get; set; }
        public string    IsLearningDisabilityDiagnosedDetails                      { get; set; }
        public int?      IsLearningDisabilityDiagnosedId                           { get; set; }
        public string    IsLearningDisabilityHardToManageDetails                   { get; set; }
        public int?      IsLearningDisabilityHardToManageId                        { get; set; }
        public string    IsLearningDisabilityHardToParticipateDetails              { get; set; }
        public int?      IsLearningDisabilityHardToParticipateId                   { get; set; }
        public int?      IsDomesticViolenceHurtingYouOrOthersId                    { get; set; }
        public string    IsDomesticViolenceHurtingYouOrOthersDetails               { get; set; }
        public int?      IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId        { get; set; }
        public string    IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails   { get; set; }
        public int?      IsDomesticViolencePartnerControlledMoneyId                { get; set; }
        public string    IsDomesticViolencePartnerControlledMoneyDetails           { get; set; }
        public int?      IsDomesticViolenceReceivedServicesOrLivedInShelterId      { get; set; }
        public string    IsDomesticViolenceReceivedServicesOrLivedInShelterDetails { get; set; }
        public int?      IsDomesticViolenceEmotionallyOrVerballyAbusingId          { get; set; }
        public string    IsDomesticViolenceEmotionallyOrVerballyAbusingDetails     { get; set; }
        public int?      IsDomesticViolenceCallingHarassingStalkingAtWorkId        { get; set; }
        public string    IsDomesticViolenceCallingHarassingStalkingAtWorkDetails   { get; set; }
        public int?      IsDomesticViolenceMakingItDifficultToWorkId               { get; set; }
        public string    IsDomesticViolenceMakingItDifficultToWorkDetails          { get; set; }
        public int?      IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId      { get; set; }
        public string    IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails { get; set; }
        public int?      IsDomesticViolenceInvolvedInCourtsId                      { get; set; }
        public string    IsDomesticViolenceInvolvedInCourtsDetails                 { get; set; }
        public DateTime? CreatedDate                                               { get; set; }

        public string Notes { get; set; }

        //public bool      IsDeleted                                                 { get; set; }    //TODO: already defined
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<BarrierDetail> BarrierDetails                                    { get; set; } = new List<BarrierDetail>();
        public virtual Participant                Participant                                       { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsAODAHardToManage                    { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsAODAHardToParticipate               { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsAODATakeTreatment                   { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsLearningDisabilityDiagnosed         { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsLearningDisabilityHardToManage      { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsLearningDisabilityHardToParticipate { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsMentalHealthDiagnosed               { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsMentalHealthHardToManage            { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsMentalHealthHardToParticipate       { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsMentalHealthTakeMedication          { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsPhysicalHealthHardHardToParticipate { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsPhysicalHealthHardToManage          { get; set; }
        public virtual YesNoRefused               YesNoRefusedIsPhysicalHealthTakeMedication        { get; set; }
        public virtual YesNoRefused               YesNoRefused3                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused4                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused5                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused6                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused7                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused8                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused9                                     { get; set; }
        public virtual YesNoRefused               YesNoRefused10                                    { get; set; }
        public virtual YesNoRefused               YesNoRefused11                                    { get; set; }

        #endregion
    }
}
