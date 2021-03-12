using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierSection : ICommonDelModel, ICloneable
    {
        int? ParticipantId { get; set; }
        int? IsPhysicalHealthHardToManageId { get; set; }
        int? IsPhysicalHealthHardToParticipateId { get; set; }
        int? IsPhysicalHealthTakeMedicationId { get; set; }
        int? IsMentalHealthHardDiagnosedId { get; set; }
        int? IsMentalHealthHardToManageId { get; set; }
        int? IsMentalHealthHardToParticipateId { get; set; }
        int? IsMentalHealthTakeMedicationId { get; set; }
        int? IsAODAHardToManageId { get; set; }
        int? IsAODAHardToParticipateId { get; set; }
        int? IsAODATakeTreatmentId { get; set; }
        int? IsLearningDisabilityDiagnosedId { get; set; }
        int? IsLearningDisabilityHardToManageId { get; set; }
        int? IsLearningDisabilityHardToParticipateId { get; set; }

        string IsPhysicalHealthHardToManageDetails { get; set; }
        string IsPhysicalHealthHardToParticipateDetails { get; set; }
        string IsPhysicalHealthTakeMedicationDetails { get; set; }
        string IsMentalHealthHardDiagnosedDetails { get; set; }
        string IsMentalHealthHardToManageDetails { get; set; }
        string IsMentalHealthHardToParticipateDetails { get; set; }
        string IsMentalHealthTakeMedicationDetails { get; set; }
        string IsAODAHardToManageDetails { get; set; }
        string IsAODAHardToParticipateDetails { get; set; }
        string IsAODATakeTreatmentDetails { get; set; }
        string IsLearningDisabilityDiagnosedDetails { get; set; }
        string IsLearningDisabilityHardToManageDetails { get; set; }
        string IsLearningDisabilityHardToParticipateDetails { get; set; }
        Nullable<int> IsDomesticViolenceHurtingYouOrOthersId { get; set; }
        string IsDomesticViolenceHurtingYouOrOthersDetails { get; set; }
        Nullable<int> IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId { get; set; }
        string IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails { get; set; }
        Nullable<int> IsDomesticViolencePartnerControlledMoneyId { get; set; }
        string IsDomesticViolencePartnerControlledMoneyDetails { get; set; }
        Nullable<int> IsDomesticViolenceReceivedServicesOrLivedInShelterId { get; set; }
        string IsDomesticViolenceReceivedServicesOrLivedInShelterDetails { get; set; }
        Nullable<int> IsDomesticViolenceEmotionallyOrVerballyAbusingId { get; set; }
        string IsDomesticViolenceEmotionallyOrVerballyAbusingDetails { get; set; }
        Nullable<int> IsDomesticViolenceCallingHarassingStalkingAtWorkId { get; set; }
        string IsDomesticViolenceCallingHarassingStalkingAtWorkDetails { get; set; }
        Nullable<int> IsDomesticViolenceMakingItDifficultToWorkId { get; set; }
        string IsDomesticViolenceMakingItDifficultToWorkDetails { get; set; }
        Nullable<int> IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId { get; set; }
        string IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails { get; set; }
        Nullable<int> IsDomesticViolenceInvolvedInCourtsId { get; set; }
        string IsDomesticViolenceInvolvedInCourtsDetails { get; set; }
        string Notes { get; set; }

        ICollection<IBarrierDetail> BarrierDetails { get; set; }
        IParticipant Participant { get; set; }

        IYesNoRefused YesNoRefusedIsAODAHardToManage { get; set; }
        IYesNoRefused YesNoRefusedIsAODAHardToParticipate { get; set; }
        IYesNoRefused YesNoRefusedIsAODATakeTreatment { get; set; }
        IYesNoRefused YesNoRefusedIsLearningDisabilityDiagnosed { get; set; }
        IYesNoRefused YesNoRefusedIsLearningDisabilityHardToManage { get; set; }
        IYesNoRefused YesNoRefusedIsLearningDisabilityHardToParticipate { get; set; }
        IYesNoRefused YesNoRefusedIsMentalHealthDiagnosed { get; set; }
        IYesNoRefused YesNoRefusedIsMentalHealthHardToManage { get; set; }
        IYesNoRefused YesNoRefusedIsMentalHealthHardToParticipate { get; set; }
        IYesNoRefused YesNoRefusedIsMentalHealthTakeMedication { get; set; }
        IYesNoRefused YesNoRefusedIsPhysicalHealthHardHardToParticipate { get; set; }
        IYesNoRefused YesNoRefusedIsPhysicalHealthHardToManage { get; set; }
        IYesNoRefused YesNoRefusedIsPhysicalHealthTakeMedication { get; set; }
        IYesNoRefused YesNoRefused3 { get; set; }
        IYesNoRefused YesNoRefused4 { get; set; }
        IYesNoRefused YesNoRefused5 { get; set; }
        IYesNoRefused YesNoRefused6 { get; set; }
        IYesNoRefused YesNoRefused7 { get; set; }
        IYesNoRefused YesNoRefused8 { get; set; }
        IYesNoRefused YesNoRefused9 { get; set; }
        IYesNoRefused YesNoRefused10 { get; set; }
        IYesNoRefused YesNoRefused11 { get; set; }
    }
}
