using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class ParticipantBarrierSectionViewModel : BaseInformalAssessmentViewModel
    {
        public ParticipantBarrierSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public ParticipantBarrierSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant);
        }

        public bool PostData(ParticipantBarrierSectionContract contract, string user)
        {
            var ia          = InformalAssessment;
            var participant = Participant;

            if (participant == null)
            {
                throw new InvalidOperationException("PIN not valid.");
            }

            if (contract == null)
            {
                throw new InvalidOperationException("Partcipant Barrier data is missing.");
            }

            if (ia == null)
            {
                throw new InvalidOperationException("InformalAssessment record is missing.");
            }

            var updateTime = DateTime.Now;

            var bs = participant.BarrierSection ?? Repo.NewBarrierSection(participant.Id, user);
            bs.ModifiedBy   = AuthUser.Username;
            bs.ModifiedDate = updateTime;
            Repo.StartChangeTracking(bs);

            // If we have an in progress assessment, then we will set the IBarrierAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            var bas = ia.BarrierAssessmentSection ?? Repo.NewBarrierAssessmentSection(ia, user);
            bas.ReviewCompleted = true;
            bas.ModifiedBy   = AuthUser.Username;
            bas.ModifiedDate = updateTime;
            Repo.StartChangeTracking(bas);

            var userRowVersion    = contract.RowVersion;
            var userAssRowVersion = contract.AssessmentRowVersion;

            // Physical Health
            bs.IsPhysicalHealthHardToManageId      = contract.IsPhysicalHealthHardToManage.Status;
            bs.IsPhysicalHealthHardToManageDetails = contract.IsPhysicalHealthHardToManage.Details;

            bs.IsPhysicalHealthHardToParticipateId      = contract.IsPhysicalHealthHardToParticipate.Status;
            bs.IsPhysicalHealthHardToParticipateDetails = contract.IsPhysicalHealthHardToParticipate.Details;

            bs.IsPhysicalHealthTakeMedicationId      = contract.IsPhysicalHealthTakeMedication.Status;
            bs.IsPhysicalHealthTakeMedicationDetails = contract.IsPhysicalHealthTakeMedication.Details;

            if (contract.IsPhysicalHealthHardToParticipate.Status == YesNoRefusedLookup.YesId || contract.IsPhysicalHealthHardToManage.Status == YesNoRefusedLookup.YesId)
            {
                bs.IsPhysicalHealthTakeMedicationId      = contract.IsPhysicalHealthTakeMedication.Status;
                bs.IsPhysicalHealthTakeMedicationDetails = contract.IsPhysicalHealthTakeMedication.Details;
            }
            else
            {
                bs.IsPhysicalHealthTakeMedicationId      = null;
                bs.IsPhysicalHealthTakeMedicationDetails = null;
            }

            // Mental Health
            bs.IsMentalHealthHardDiagnosedId      = contract.IsMentalHealthDiagnosed.Status;
            bs.IsMentalHealthHardDiagnosedDetails = contract.IsMentalHealthDiagnosed.Details;

            bs.IsMentalHealthHardToManageId      = contract.IsMentalHealthHardToManage.Status;
            bs.IsMentalHealthHardToManageDetails = contract.IsMentalHealthHardToManage.Details;

            bs.IsMentalHealthHardToParticipateId      = contract.IsMentalHealthHardToParticipate.Status;
            bs.IsMentalHealthHardToParticipateDetails = contract.IsMentalHealthHardToParticipate.Details;

            if (contract.IsMentalHealthDiagnosed.Status         == YesNoRefusedLookup.YesId || contract.IsMentalHealthHardToManage.Status == YesNoRefusedLookup.YesId ||
                contract.IsMentalHealthHardToParticipate.Status == YesNoRefusedLookup.YesId)
            {
                bs.IsMentalHealthTakeMedicationId      = contract.IsMentalHealthTakeMedication.Status;
                bs.IsMentalHealthTakeMedicationDetails = contract.IsMentalHealthTakeMedication.Details;
            }
            else
            {
                bs.IsMentalHealthTakeMedicationId      = null;
                bs.IsMentalHealthTakeMedicationDetails = null;
            }

            // big hammer for bug fix: 1589
            if (contract.IsMentalHealthTakeMedication.Status == YesNoRefusedLookup.NoId)
            {
                contract.IsMentalHealthTakeMedication.Details = null;
                bs.IsMentalHealthTakeMedicationDetails        = null;
            }

            // AODA
            bs.IsAODAHardToManageId      = contract.IsAodaHardToManage.Status;
            bs.IsAODAHardToManageDetails = contract.IsAodaHardToManage.Details;

            // Currently in any alcohol or drug treatment services.
            bs.IsAODAHardToParticipateId      = contract.IsAodaHardToParticipate.Status;
            bs.IsAODAHardToParticipateDetails = contract.IsAodaHardToParticipate.Details;

            if (contract.IsAodaHardToManage.Status == YesNoRefusedLookup.YesId || contract.IsAodaHardToParticipate.Status == YesNoRefusedLookup.YesId)
            {
                bs.IsAODATakeTreatmentId      = contract.IsAodaTakeTreatment.Status;
                bs.IsAODATakeTreatmentDetails = contract.IsAodaTakeTreatment.Details;
            }
            else
            {
                bs.IsAODATakeTreatmentId      = null;
                bs.IsAODATakeTreatmentDetails = null;
            }

            bs.IsDomesticViolenceHurtingYouOrOthersId                    = contract.IsDomesticViolenceHurtingYouOrOthers.Status;
            bs.IsDomesticViolenceHurtingYouOrOthersDetails               = contract.IsDomesticViolenceHurtingYouOrOthers.Details;
            bs.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId        = contract.IsDomesticViolenceEverHarmedPhysicallyOrSexually.Status;
            bs.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails   = contract.IsDomesticViolenceEverHarmedPhysicallyOrSexually.Details;
            bs.IsDomesticViolencePartnerControlledMoneyId                = contract.IsDomesticViolencePartnerControlledMoney.Status;
            bs.IsDomesticViolencePartnerControlledMoneyDetails           = contract.IsDomesticViolencePartnerControlledMoney.Details;
            bs.IsDomesticViolenceReceivedServicesOrLivedInShelterId      = contract.IsDomesticViolenceReceivedServicesOrLivedInShelter.Status;
            bs.IsDomesticViolenceReceivedServicesOrLivedInShelterDetails = contract.IsDomesticViolenceReceivedServicesOrLivedInShelter.Details;
            bs.IsDomesticViolenceEmotionallyOrVerballyAbusingId          = contract.IsDomesticViolenceEmotionallyOrVerballyAbusing.Status;
            bs.IsDomesticViolenceEmotionallyOrVerballyAbusingDetails     = contract.IsDomesticViolenceEmotionallyOrVerballyAbusing.Details;
            bs.IsDomesticViolenceCallingHarassingStalkingAtWorkId        = contract.IsDomesticViolenceCallingHarassingStalkingAtWork.Status;
            bs.IsDomesticViolenceCallingHarassingStalkingAtWorkDetails   = contract.IsDomesticViolenceCallingHarassingStalkingAtWork.Details;
            bs.IsDomesticViolenceMakingItDifficultToWorkId               = contract.IsDomesticViolenceMakingItDifficultToWork.Status;
            bs.IsDomesticViolenceMakingItDifficultToWorkDetails          = contract.IsDomesticViolenceMakingItDifficultToWork.Details;
            bs.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId      = contract.IsDomesticViolenceOverwhelmedByRapeOrSexualAssault.Status;
            bs.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails = contract.IsDomesticViolenceOverwhelmedByRapeOrSexualAssault.Details;
            bs.IsDomesticViolenceInvolvedInCourtsId                      = contract.IsDomesticViolenceInvolvedInCourts.Status;
            bs.IsDomesticViolenceInvolvedInCourtsDetails                 = contract.IsDomesticViolenceInvolvedInCourts.Details;

            // Cognitive and Learning Needs
            bs.IsLearningDisabilityDiagnosedId      = contract.IsLearningDisabilityDiagnosed.Status;
            bs.IsLearningDisabilityDiagnosedDetails = contract.IsLearningDisabilityDiagnosed.Details;

            bs.IsLearningDisabilityHardToManageId      = contract.IsLearningDisabilityHardToManage.Status;
            bs.IsLearningDisabilityHardToManageDetails = contract.IsLearningDisabilityHardToManage.Details;

            bs.IsLearningDisabilityHardToParticipateId      = contract.IsLearningDisabilityHardToParticipate.Status;
            bs.IsLearningDisabilityHardToParticipateDetails = contract.IsLearningDisabilityHardToParticipate.Details;

            bs.Notes = contract.Notes;

            var currentIA = Repo.GetMostRecentAssessment(participant);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;


            //Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(bs, userRowVersion))
            {
                return false;
            }

            if (!Repo.IsRowVersionStillCurrent(bas, userAssRowVersion))
            {
                return false;
            }

            // If the first save completes, it actually has already saved the ParticipantbarrierAssessmentSection
            // object as well since they are on the save repository context.  But if the ChildYouthSection didn't
            // need saving, we still need to SaveIfChangd on the ParticipantBarrierAssessmentSection.
            if (!Repo.SaveIfChanged(bs, user))
            {
                Repo.SaveIfChanged(bas, user);
            }

            return true;
        }

        public static ParticipantBarrierSectionContract GetContract(IInformalAssessment ia, IParticipant pa)
        {
            var contract = new ParticipantBarrierSectionContract();

            // Do a quick sanity check to be sure the IA is not null.  If it
            // is we'll send back a completely empty section.
            if (ia != null && pa != null)
            {
                if (pa.BarrierSection != null)
                {
                    var pbs = pa.BarrierSection;
                    contract.Barriers = new List<BarrierDetailContract>();

                    contract.RowVersion   = pbs.RowVersion;
                    contract.ModifiedBy   = pbs.ModifiedBy;
                    contract.ModifiedDate = pbs.ModifiedDate;

                    // Physical Health.
                    contract.IsPhysicalHealthHardToManage      = new YesOrNoRefusedContract(pbs.IsPhysicalHealthHardToManageId,      pbs.YesNoRefusedIsPhysicalHealthHardToManage?.Name,          pbs.IsPhysicalHealthHardToManageDetails);
                    contract.IsPhysicalHealthHardToParticipate = new YesOrNoRefusedContract(pbs.IsPhysicalHealthHardToParticipateId, pbs.YesNoRefusedIsPhysicalHealthHardHardToParticipate?.Name, pbs.IsPhysicalHealthHardToParticipateDetails);
                    contract.IsPhysicalHealthTakeMedication    = new YesOrNoRefusedContract(pbs.IsPhysicalHealthTakeMedicationId,    pbs.YesNoRefusedIsPhysicalHealthTakeMedication?.Name,        pbs.IsPhysicalHealthTakeMedicationDetails);

                    // Mental Health.
                    contract.IsMentalHealthDiagnosed         = new YesOrNoRefusedContract(pbs.IsMentalHealthHardDiagnosedId,     pbs.YesNoRefusedIsMentalHealthDiagnosed?.Name,         pbs.IsMentalHealthHardDiagnosedDetails);
                    contract.IsMentalHealthHardToParticipate = new YesOrNoRefusedContract(pbs.IsMentalHealthHardToParticipateId, pbs.YesNoRefusedIsMentalHealthHardToParticipate?.Name, pbs.IsMentalHealthHardToParticipateDetails);
                    contract.IsMentalHealthHardToManage      = new YesOrNoRefusedContract(pbs.IsMentalHealthHardToManageId,      pbs.YesNoRefusedIsMentalHealthHardToManage?.Name,      pbs.IsMentalHealthHardToManageDetails);
                    contract.IsMentalHealthTakeMedication    = new YesOrNoRefusedContract(pbs.IsMentalHealthTakeMedicationId,    pbs.YesNoRefusedIsMentalHealthTakeMedication?.Name,    pbs.IsMentalHealthTakeMedicationDetails);

                    // AODA.
                    contract.IsAodaHardToManage      = new YesOrNoRefusedContract(pbs.IsAODAHardToManageId,      pbs.YesNoRefusedIsAODAHardToManage?.Name,      pbs.IsAODAHardToManageDetails);
                    contract.IsAodaHardToParticipate = new YesOrNoRefusedContract(pbs.IsAODAHardToParticipateId, pbs.YesNoRefusedIsAODAHardToParticipate?.Name, pbs.IsAODAHardToParticipateDetails);
                    contract.IsAodaTakeTreatment     = new YesOrNoRefusedContract(pbs.IsAODATakeTreatmentId,     pbs.YesNoRefusedIsAODATakeTreatment?.Name,     pbs.IsAODATakeTreatmentDetails);

                    // Domestic Violence.
                    contract.IsDomesticViolenceHurtingYouOrOthers               = new YesOrNoRefusedContract(pbs.IsDomesticViolenceHurtingYouOrOthersId,               pbs.YesNoRefused6?.Name,  pbs.IsDomesticViolenceHurtingYouOrOthersDetails);
                    contract.IsDomesticViolenceEverHarmedPhysicallyOrSexually   = new YesOrNoRefusedContract(pbs.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId,   pbs.YesNoRefused5?.Name,  pbs.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails);
                    contract.IsDomesticViolencePartnerControlledMoney           = new YesOrNoRefusedContract(pbs.IsDomesticViolencePartnerControlledMoneyId,           pbs.YesNoRefused10?.Name, pbs.IsDomesticViolencePartnerControlledMoneyDetails);
                    contract.IsDomesticViolenceReceivedServicesOrLivedInShelter = new YesOrNoRefusedContract(pbs.IsDomesticViolenceReceivedServicesOrLivedInShelterId, pbs.YesNoRefused11?.Name, pbs.IsDomesticViolenceReceivedServicesOrLivedInShelterDetails);
                    contract.IsDomesticViolenceEmotionallyOrVerballyAbusing     = new YesOrNoRefusedContract(pbs.IsDomesticViolenceEmotionallyOrVerballyAbusingId,     pbs.YesNoRefused4?.Name,  pbs.IsDomesticViolenceEmotionallyOrVerballyAbusingDetails);
                    contract.IsDomesticViolenceCallingHarassingStalkingAtWork   = new YesOrNoRefusedContract(pbs.IsDomesticViolenceCallingHarassingStalkingAtWorkId,   pbs.YesNoRefused3?.Name,  pbs.IsDomesticViolenceCallingHarassingStalkingAtWorkDetails);
                    contract.IsDomesticViolenceMakingItDifficultToWork          = new YesOrNoRefusedContract(pbs.IsDomesticViolenceMakingItDifficultToWorkId,          pbs.YesNoRefused8?.Name,  pbs.IsDomesticViolenceMakingItDifficultToWorkDetails);
                    contract.IsDomesticViolenceOverwhelmedByRapeOrSexualAssault = new YesOrNoRefusedContract(pbs.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId, pbs.YesNoRefused9?.Name,  pbs.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails);
                    contract.IsDomesticViolenceInvolvedInCourts                 = new YesOrNoRefusedContract(pbs.IsDomesticViolenceInvolvedInCourtsId,                 pbs.YesNoRefused7?.Name,  pbs.IsDomesticViolenceInvolvedInCourtsDetails);

                    // Learning Disablity.
                    contract.IsLearningDisabilityDiagnosed         = new YesOrNoRefusedContract(pbs.IsLearningDisabilityDiagnosedId,         pbs.YesNoRefusedIsLearningDisabilityDiagnosed?.Name,         pbs.IsLearningDisabilityDiagnosedDetails);
                    contract.IsLearningDisabilityHardToManage      = new YesOrNoRefusedContract(pbs.IsLearningDisabilityHardToManageId,      pbs.YesNoRefusedIsLearningDisabilityHardToManage?.Name,      pbs.IsLearningDisabilityHardToManageDetails);
                    contract.IsLearningDisabilityHardToParticipate = new YesOrNoRefusedContract(pbs.IsLearningDisabilityHardToParticipateId, pbs.YesNoRefusedIsLearningDisabilityHardToParticipate?.Name, pbs.IsLearningDisabilityHardToParticipateDetails);

                    contract.Notes = pbs.Notes;
                }

                if (ia.BarrierAssessmentSection != null)
                {
                    contract.IsSafeAppropriateToAsk   = false;
                    contract.IsSubmittedViaDriverFlow = true;

                    // Make a call to the helper method in the base class to set the assessment row version and update
                    // modified stuff if needed.
                    UpdateRowVersionAndModifiedIfAssessmentMoreRecent(contract, pa.BarrierSection, ia.BarrierAssessmentSection);
                }
            }

            return contract;
        }
    }
}
