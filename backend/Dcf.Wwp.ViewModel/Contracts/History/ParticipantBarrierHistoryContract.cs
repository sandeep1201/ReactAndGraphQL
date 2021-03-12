using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    class ParticipantBarrierHistoryContract
    {
        public List<HistoryValueContract> IsPhysicalHealthHardToManageDetails { get; set; }
        public List<HistoryValueContract> IsPhysicalHealthHardToManageId { get; set; }
        public List<HistoryValueContract> IsPhysicalHealthHardToParticipateDetails { get; set; }
        public List<HistoryValueContract> IsPhysicalHealthHardToParticipateId { get; set; }
        public List<HistoryValueContract> IsPhysicalHealthTakeMedicationDetails { get; set; }
        public List<HistoryValueContract> IsPhysicalHealthTakeMedicationId { get; set; }
        public List<HistoryValueContract> IsMentalHealthHardDiagnosedDetails { get; set; }
        public List<HistoryValueContract> IsMentalHealthHardDiagnosedId { get; set; }
        public List<HistoryValueContract> IsMentalHealthHardToManageDetails { get; set; }
        public List<HistoryValueContract> IsMentalHealthHardToManageId { get; set; }
        public List<HistoryValueContract> IsMentalHealthHardToParticipateDetails { get; set; }
        public List<HistoryValueContract> IsMentalHealthHardToParticipateId { get; set; }
        public List<HistoryValueContract> IsMentalHealthTakeMedicationDetails { get; set; }
        public List<HistoryValueContract> IsMentalHealthTakeMedicationId { get; set; }
        public List<HistoryValueContract> IsAODAHardToManageDetails { get; set; }
        public List<HistoryValueContract> IsAODAHardToManageId { get; set; }
        public List<HistoryValueContract> IsAODAHardToParticipateDetails { get; set; }
        public List<HistoryValueContract> IsAODAHardToParticipateId { get; set; }
        public List<HistoryValueContract> IsAODATakeTreatmentDetails { get; set; }
        public List<HistoryValueContract> IsAODATakeTreatmentId { get; set; }
        public List<HistoryValueContract> IsDomesticViolenceFamilySafetyDetails { get; set; }
        public List<HistoryValueContract> IsDomesticViolenceFamilySafetyId { get; set; }
        public List<HistoryValueContract> IsDomesticViolenceHouseholdSupportiveDetails { get; set; }
        public List<HistoryValueContract> IsDomesticViolenceHouseholdSupportiveId { get; set; }
        public List<HistoryValueContract> IsDomesticViolenceHouseholdPreventiveDetails { get; set; }
        public List<HistoryValueContract> IsDomesticViolenceHouseholdPreventiveId { get; set; }
        public List<HistoryValueContract> IsLearningDisabilityDiagnosedDetails { get; set; }
        public List<HistoryValueContract> IsLearningDisabilityDiagnosedId { get; set; }
        public List<HistoryValueContract> IsLearningDisabilityHardToManageDetails { get; set; }
        public List<HistoryValueContract> IsLearningDisabilityHardToManageId { get; set; }
        public List<HistoryValueContract> IsLearningDisabilityHardToParticipateDetails { get; set; }
        public List<HistoryValueContract> IsLearningDisabilityHardToParticipateId { get; set; }

        public List<HistoryValueContract> Notes { get; set; }
    }
}
