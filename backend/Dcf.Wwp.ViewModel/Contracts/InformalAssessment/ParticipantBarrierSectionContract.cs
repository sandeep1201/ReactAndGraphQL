using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class ParticipantBarrierSectionContract : BaseInformalAssessmentContract
    {
        public YesOrNoRefusedContract IsPhysicalHealthHardToManage { get; set; }

        public YesOrNoRefusedContract IsPhysicalHealthHardToParticipate { get; set; }

        public YesOrNoRefusedContract IsPhysicalHealthTakeMedication { get; set; }

        public YesOrNoRefusedContract IsMentalHealthDiagnosed { get; set; }

        public YesOrNoRefusedContract IsMentalHealthHardToManage { get; set; }

        public YesOrNoRefusedContract IsMentalHealthHardToParticipate { get; set; }

        public YesOrNoRefusedContract IsMentalHealthTakeMedication { get; set; }

        public YesOrNoRefusedContract IsAodaHardToManage { get; set; }

        public YesOrNoRefusedContract IsAodaHardToParticipate { get; set; }

        public YesOrNoRefusedContract IsAodaTakeTreatment { get; set; }

        public YesOrNoRefusedContract IsDomesticViolenceHurtingYouOrOthers { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceEverHarmedPhysicallyOrSexually { get; set; }
        public YesOrNoRefusedContract IsDomesticViolencePartnerControlledMoney { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceReceivedServicesOrLivedInShelter { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceEmotionallyOrVerballyAbusing { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceCallingHarassingStalkingAtWork { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceMakingItDifficultToWork { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceOverwhelmedByRapeOrSexualAssault { get; set; }
        public YesOrNoRefusedContract IsDomesticViolenceInvolvedInCourts { get; set; }

        public YesOrNoRefusedContract IsLearningDisabilityDiagnosed { get; set; }

        public YesOrNoRefusedContract IsLearningDisabilityHardToManage { get; set; }

        public YesOrNoRefusedContract IsLearningDisabilityHardToParticipate { get; set; }

        public bool? IsSafeAppropriateToAsk { get; set; }

        public string Notes { get; set; }

        public List<BarrierDetailContract> Barriers { get; set; }

        //[DataMember(Name = "mentalHealthBarriers")]
        //public List<BarrierDetailContract> MentalHealthBarriers { get; set; }

        //[DataMember(Name = "AodBarriers")]
        //public List<BarrierDetailContract> AodBarriers { get; set; }

        //[DataMember(Name = "physicalHealthBarriers")]
        //public List<BarrierDetailContract> PhysicalHealthBarriers { get; set; }

        //[DataMember(Name = "domesticVoilenceBarriers")]
        //public List<BarrierDetailContract> DomesticVoilenceBarriers { get; set; }

        //[DataMember(Name = "cognitiveAndLearningBarriers")]
        //public List<BarrierDetailContract> CognitiveAndLearningBarriers { get; set; }
    }
}
