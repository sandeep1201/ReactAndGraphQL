using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierSection : BaseCommonModel, IBarrierSection
    {
        ICollection<IBarrierDetail> IBarrierSection.BarrierDetails
        {
            get { return BarrierDetails.Cast<IBarrierDetail>().ToList(); }
            set { BarrierDetails = (ICollection<BarrierDetail>)value; }
        }

        IParticipant IBarrierSection.Participant
        {
            get { return Participant; }
            set { Participant = (Participant)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsAODAHardToManage
        {
            get { return YesNoRefusedIsAODAHardToManage; }
            set { YesNoRefusedIsAODAHardToManage = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsAODAHardToParticipate
        {
            get { return YesNoRefusedIsAODAHardToParticipate; }
            set { YesNoRefusedIsAODAHardToParticipate = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsAODATakeTreatment
        {
            get { return YesNoRefusedIsAODATakeTreatment; }
            set { YesNoRefusedIsAODATakeTreatment = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsLearningDisabilityDiagnosed
        {
            get { return YesNoRefusedIsLearningDisabilityDiagnosed; }
            set { YesNoRefusedIsLearningDisabilityDiagnosed = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsLearningDisabilityHardToManage
        {
            get { return YesNoRefusedIsLearningDisabilityHardToManage; }
            set { YesNoRefusedIsLearningDisabilityHardToManage = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsLearningDisabilityHardToParticipate
        {
            get { return YesNoRefusedIsLearningDisabilityHardToParticipate; }
            set { YesNoRefusedIsLearningDisabilityHardToParticipate = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsMentalHealthDiagnosed
        {
            get { return YesNoRefusedIsMentalHealthDiagnosed; }
            set { YesNoRefusedIsMentalHealthDiagnosed = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsMentalHealthHardToManage
        {
            get { return YesNoRefusedIsMentalHealthHardToManage; }
            set { YesNoRefusedIsMentalHealthHardToManage = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsMentalHealthHardToParticipate
        {
            get { return YesNoRefusedIsMentalHealthHardToParticipate; }
            set { YesNoRefusedIsMentalHealthHardToParticipate = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsMentalHealthTakeMedication
        {
            get { return YesNoRefusedIsMentalHealthTakeMedication; }
            set { YesNoRefusedIsMentalHealthTakeMedication = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsPhysicalHealthHardHardToParticipate
        {
            get { return YesNoRefusedIsPhysicalHealthHardHardToParticipate; }
            set { YesNoRefusedIsPhysicalHealthHardHardToParticipate = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsPhysicalHealthHardToManage
        {
            get { return YesNoRefusedIsPhysicalHealthHardToManage; }
            set { YesNoRefusedIsPhysicalHealthHardToManage = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefusedIsPhysicalHealthTakeMedication
        {
            get { return YesNoRefusedIsPhysicalHealthTakeMedication; }
            set { YesNoRefusedIsPhysicalHealthTakeMedication = (YesNoRefused)value; }
        }

        IYesNoRefused IBarrierSection.YesNoRefused3
        {
            get { return YesNoRefused3; }
            set { YesNoRefused3 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused4
        {
            get { return YesNoRefused4; }
            set { YesNoRefused4 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused5
        {
            get { return YesNoRefused5; }
            set { YesNoRefused5 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused6
        {
            get { return YesNoRefused6; }
            set { YesNoRefused6 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused7
        {
            get { return YesNoRefused7; }
            set { YesNoRefused7 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused8
        {
            get { return YesNoRefused8; }
            set { YesNoRefused8 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused9
        {
            get { return YesNoRefused9; }
            set { YesNoRefused9 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused10
        {
            get { return YesNoRefused10; }
            set { YesNoRefused10 = (YesNoRefused)value; }
        }
        IYesNoRefused IBarrierSection.YesNoRefused11
        {
            get { return YesNoRefused11; }
            set { YesNoRefused11 = (YesNoRefused)value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new BarrierSection
                        {
                            IsPhysicalHealthHardToManageId                            = IsPhysicalHealthHardToManageId,
                            IsPhysicalHealthHardToManageDetails                       = IsPhysicalHealthHardToManageDetails,
                            IsPhysicalHealthHardToParticipateId                       = IsPhysicalHealthHardToParticipateId,
                            IsPhysicalHealthHardToParticipateDetails                  = IsPhysicalHealthHardToParticipateDetails,
                            IsPhysicalHealthTakeMedicationId                          = IsPhysicalHealthTakeMedicationId,
                            IsPhysicalHealthTakeMedicationDetails                     = IsPhysicalHealthTakeMedicationDetails,
                            IsMentalHealthHardDiagnosedId                             = IsMentalHealthHardDiagnosedId,
                            IsMentalHealthHardDiagnosedDetails                        = IsMentalHealthHardDiagnosedDetails,
                            IsMentalHealthHardToManageId                              = IsMentalHealthHardToManageId,
                            IsMentalHealthHardToManageDetails                         = IsMentalHealthHardToManageDetails,
                            IsMentalHealthHardToParticipateId                         = IsMentalHealthHardToParticipateId,
                            IsMentalHealthHardToParticipateDetails                    = IsMentalHealthHardToParticipateDetails,
                            IsMentalHealthTakeMedicationId                            = IsMentalHealthTakeMedicationId,
                            IsMentalHealthTakeMedicationDetails                       = IsMentalHealthTakeMedicationDetails,
                            IsAODAHardToManageId                                      = IsAODAHardToManageId,
                            IsAODAHardToManageDetails                                 = IsAODAHardToManageDetails,
                            IsAODAHardToParticipateId                                 = IsAODAHardToParticipateId,
                            IsAODAHardToParticipateDetails                            = IsAODAHardToParticipateDetails,
                            IsAODATakeTreatmentId                                     = IsAODATakeTreatmentId,
                            IsAODATakeTreatmentDetails                                = IsAODATakeTreatmentDetails,
                            IsLearningDisabilityDiagnosedId                           = IsLearningDisabilityDiagnosedId,
                            IsLearningDisabilityDiagnosedDetails                      = IsLearningDisabilityDiagnosedDetails,
                            IsLearningDisabilityHardToManageId                        = IsLearningDisabilityHardToManageId,
                            IsLearningDisabilityHardToManageDetails                   = IsLearningDisabilityHardToManageDetails,
                            IsLearningDisabilityHardToParticipateId                   = IsLearningDisabilityHardToParticipateId,
                            IsLearningDisabilityHardToParticipateDetails              = IsLearningDisabilityHardToParticipateDetails,
                            IsDomesticViolenceHurtingYouOrOthersId                    = IsDomesticViolenceHurtingYouOrOthersId,
                            IsDomesticViolenceHurtingYouOrOthersDetails               = IsDomesticViolenceHurtingYouOrOthersDetails,
                            IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId        = IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId,
                            IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails   = IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails,
                            IsDomesticViolencePartnerControlledMoneyId                = IsDomesticViolencePartnerControlledMoneyId,
                            IsDomesticViolencePartnerControlledMoneyDetails           = IsDomesticViolencePartnerControlledMoneyDetails,
                            IsDomesticViolenceReceivedServicesOrLivedInShelterId      = IsDomesticViolenceReceivedServicesOrLivedInShelterId,
                            IsDomesticViolenceReceivedServicesOrLivedInShelterDetails = IsDomesticViolenceReceivedServicesOrLivedInShelterDetails,
                            IsDomesticViolenceEmotionallyOrVerballyAbusingId          = IsDomesticViolenceEmotionallyOrVerballyAbusingId,
                            IsDomesticViolenceEmotionallyOrVerballyAbusingDetails     = IsDomesticViolenceEmotionallyOrVerballyAbusingDetails,
                            IsDomesticViolenceCallingHarassingStalkingAtWorkId        = IsDomesticViolenceCallingHarassingStalkingAtWorkId,
                            IsDomesticViolenceCallingHarassingStalkingAtWorkDetails   = IsDomesticViolenceCallingHarassingStalkingAtWorkDetails,
                            IsDomesticViolenceMakingItDifficultToWorkId               = IsDomesticViolenceMakingItDifficultToWorkId,
                            IsDomesticViolenceMakingItDifficultToWorkDetails          = IsDomesticViolenceMakingItDifficultToWorkDetails,
                            IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId      = IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId,
                            IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails = IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails,
                            IsDomesticViolenceInvolvedInCourtsId                      = IsDomesticViolenceInvolvedInCourtsId,
                            IsDomesticViolenceInvolvedInCourtsDetails                 = IsDomesticViolenceInvolvedInCourtsDetails,
                            ParticipantId                                             = ParticipantId,
                            Notes                                                     = Notes
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as BarrierSection;

            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal.

            if (!AdvEqual(ParticipantId, other.ParticipantId))
            {
                return false;
            }

            if (!AdvEqual(IsPhysicalHealthHardToManageId, other.IsPhysicalHealthHardToManageId))
            {
                return false;
            }

            if (!AdvEqual(IsPhysicalHealthHardToManageDetails, other.IsPhysicalHealthHardToManageDetails))
            {
                return false;
            }

            if (!AdvEqual(IsPhysicalHealthHardToParticipateId, other.IsPhysicalHealthHardToParticipateId))
            {
                return false;
            }

            if (!AdvEqual(IsPhysicalHealthHardToParticipateDetails, other.IsPhysicalHealthHardToParticipateDetails))
            {
                return false;
            }

            if (!AdvEqual(IsPhysicalHealthTakeMedicationId, other.IsPhysicalHealthTakeMedicationId))
            {
                return false;
            }

            if (!AdvEqual(IsPhysicalHealthTakeMedicationDetails, other.IsPhysicalHealthTakeMedicationDetails))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthHardDiagnosedId, other.IsMentalHealthHardDiagnosedId))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthHardDiagnosedDetails, other.IsMentalHealthHardDiagnosedDetails))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthHardToManageId, other.IsMentalHealthHardToManageId))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthHardToManageDetails, other.IsMentalHealthHardToManageDetails))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthHardToParticipateId, other.IsMentalHealthHardToParticipateId))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthHardToParticipateDetails, other.IsMentalHealthHardToParticipateDetails))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthTakeMedicationId, other.IsMentalHealthTakeMedicationId))
            {
                return false;
            }

            if (!AdvEqual(IsMentalHealthTakeMedicationDetails, other.IsMentalHealthTakeMedicationDetails))
            {
                return false;
            }

            if (!AdvEqual(IsAODAHardToManageId, other.IsAODAHardToManageId))
            {
                return false;
            }

            if (!AdvEqual(IsAODAHardToManageDetails, other.IsAODAHardToManageDetails))
            {
                return false;
            }

            if (!AdvEqual(IsAODAHardToParticipateId, other.IsAODAHardToParticipateId))
            {
                return false;
            }

            if (!AdvEqual(IsAODAHardToParticipateDetails, other.IsAODAHardToParticipateDetails))
            {
                return false;
            }

            if (!AdvEqual(IsAODATakeTreatmentId, other.IsAODATakeTreatmentId))
            {
                return false;
            }

            if (!AdvEqual(IsAODATakeTreatmentDetails, other.IsAODATakeTreatmentDetails))
            {
                return false;
            }

            if (!AdvEqual(IsLearningDisabilityDiagnosedId, other.IsLearningDisabilityDiagnosedId))
            {
                return false;
            }

            if (!AdvEqual(IsLearningDisabilityDiagnosedDetails, other.IsLearningDisabilityDiagnosedDetails))
            {
                return false;
            }

            if (!AdvEqual(IsLearningDisabilityHardToManageId, other.IsLearningDisabilityHardToManageId))
            {
                return false;
            }

            if (!AdvEqual(IsLearningDisabilityHardToManageDetails, other.IsLearningDisabilityHardToManageDetails))
            {
                return false;
            }

            if (!AdvEqual(IsLearningDisabilityHardToParticipateId, other.IsLearningDisabilityHardToParticipateId))
            {
                return false;
            }

            if (!AdvEqual(IsLearningDisabilityHardToParticipateDetails, other.IsLearningDisabilityHardToParticipateDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceHurtingYouOrOthersId, other.IsDomesticViolenceHurtingYouOrOthersId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceHurtingYouOrOthersDetails, other.IsDomesticViolenceHurtingYouOrOthersDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId, other.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails, other.IsDomesticViolenceEverHarmedPhysicallyOrSexuallyDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolencePartnerControlledMoneyId, other.IsDomesticViolencePartnerControlledMoneyId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolencePartnerControlledMoneyDetails, other.IsDomesticViolencePartnerControlledMoneyDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceReceivedServicesOrLivedInShelterId, other.IsDomesticViolenceReceivedServicesOrLivedInShelterId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceReceivedServicesOrLivedInShelterDetails, other.IsDomesticViolenceReceivedServicesOrLivedInShelterDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceEmotionallyOrVerballyAbusingId, other.IsDomesticViolenceEmotionallyOrVerballyAbusingId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceEmotionallyOrVerballyAbusingDetails, other.IsDomesticViolenceEmotionallyOrVerballyAbusingDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceCallingHarassingStalkingAtWorkId, other.IsDomesticViolenceCallingHarassingStalkingAtWorkId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceCallingHarassingStalkingAtWorkDetails, other.IsDomesticViolenceCallingHarassingStalkingAtWorkDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceMakingItDifficultToWorkId, other.IsDomesticViolenceMakingItDifficultToWorkId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceMakingItDifficultToWorkDetails, other.IsDomesticViolenceMakingItDifficultToWorkDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId, other.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails, other.IsDomesticViolenceOverwhelmedByRapeOrSexualAssaultDetails))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceInvolvedInCourtsId, other.IsDomesticViolenceInvolvedInCourtsId))
            {
                return false;
            }

            if (!AdvEqual(IsDomesticViolenceInvolvedInCourtsDetails, other.IsDomesticViolenceInvolvedInCourtsDetails))
            {
                return false;
            }

            if (!AdvEqual(Notes, other.Notes))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
