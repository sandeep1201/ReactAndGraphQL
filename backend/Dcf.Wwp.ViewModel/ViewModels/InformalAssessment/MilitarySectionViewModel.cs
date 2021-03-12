using System;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class MilitarySectionViewModel : BaseInformalAssessmentViewModel
    {
        public MilitarySectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public MilitarySectionContract GetData() => GetContract(InformalAssessment);

        public bool PostData(MilitarySectionContract contract, string user)
        {
            var p = Participant;

            if (p == null)
            {
                throw new InvalidOperationException("PIN not valid.");
            }

            if (contract == null)
            {
                throw new InvalidOperationException("Military Training data is missing.");
            }

            IMilitaryTrainingSection           ms  = null;
            IMilitaryTrainingAssessmentSection mas = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                mas = p.InProgressInformalAssessment.MilitaryTrainingAssessmentSection ?? Repo.NewMilitaryTrainingAssessmentSection(p.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(mas);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                mas.ReviewCompleted = true;
                mas.ModifiedBy      = AuthUser.Username;
                mas.ModifiedDate    = updateTime;
            }

            ms = p.MilitaryTrainingSection ?? Repo.NewMilitaryTrainingSection(p, user);

            Repo.StartChangeTracking(ms);

            ms.ModifiedBy   = AuthUser.Username;
            ms.ModifiedDate = updateTime;

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            ms.DoesHaveTraining = contract.HasTraining;

            if (contract.HasTraining.HasValue && contract.HasTraining.Value)
            {
                if (contract.BranchId == 7)
                {
                    ms.MilitaryBranchId      = contract.BranchId;
                    ms.MilitaryRankId        = null;
                    ms.Rate                  = null;
                    ms.MilitaryDischargeType = null;
                }
                else
                {
                    ms.MilitaryBranchId = contract.BranchId;
                    ms.MilitaryRankId   = contract.RankId;
                    ms.Rate             = contract.Rate;
                }

                ms.IsCurrentlyEnlisted = contract.IsCurrentlyEnlisted;
                ms.EnlistmentDate      = contract.EnlistmentDate.ToDateTimeMonthYear();
                ms.DischargeDate       = contract.DischargeDate.ToDateTimeMonthYear();
                ms.PolarLookupId       = contract.IsEligibleForBenefitsYesNoUnknown;
                ms.BenefitsDetails     = contract.BenefitsDetails;

                if (contract.IsCurrentlyEnlisted != true)
                {
                    ms.MilitaryDischargeTypeId = contract.DischargeTypeId;
                }
                else
                {
                    ms.MilitaryDischargeTypeId = null;
                    ms.DischargeDate           = null;
                }

                ms.SkillsAndTraining = contract.SkillsAndTraining;
            }
            else
            {
                ms.MilitaryRankId          = null;
                ms.MilitaryBranchId        = null;
                ms.MilitaryDischargeTypeId = null;
                ms.Rate                    = null;
                ms.SkillsAndTraining       = null;
                ms.IsCurrentlyEnlisted     = null;
                ms.EnlistmentDate          = null;
                ms.DischargeDate           = null;
                ms.PolarLookupId           = null;
                ms.BenefitsDetails         = null;
            }

            ms.Notes = contract.Notes;

            if (p.InProgressInformalAssessment != null)
            {
                var currentIA = Repo.GetMostRecentAssessment(p);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;
            }

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(ms, userRowVersion))
            {
                return false;
            }

            if (!Repo.IsRowVersionStillCurrent(mas, userAssessRowVersion))
            {
                return false;
            }

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.
            if (!Repo.SaveIfChanged(ms, user))
            {
                Repo.SaveIfChanged(mas, user);
            }

            return true;
        }

        public static MilitarySectionContract GetContract(IInformalAssessment ia)
        {
            var contract = new MilitarySectionContract();

            if (ia != null)
            {
                if (ia.Participant?.MilitaryTrainingSection != null)
                {
                    var mt = ia.Participant.MilitaryTrainingSection;

                    contract.RowVersion   = mt.RowVersion;
                    contract.ModifiedBy   = mt.ModifiedBy;
                    contract.ModifiedDate = mt.ModifiedDate;

                    contract.HasTraining = mt.DoesHaveTraining;

                    if (contract.HasTraining.HasValue && contract.HasTraining.Value)
                    {
                        contract.BranchId                              = mt.MilitaryBranchId;
                        contract.BranchName                            = mt.MilitaryBranch?.Name?.SafeTrim();
                        contract.RankId                                = mt.MilitaryRankId;
                        contract.RankName                              = mt.MilitaryRank?.Name.SafeTrim();
                        contract.Rate                                  = mt.Rate;
                        contract.EnlistmentDate                        = mt.EnlistmentDate.ToStringMonthYear();
                        contract.IsCurrentlyEnlisted                   = mt.IsCurrentlyEnlisted;
                        contract.DischargeDate                         = mt.DischargeDate.ToStringMonthYear();
                        contract.DischargeTypeId                       = mt.MilitaryDischargeTypeId;
                        contract.DischargeTypeName                     = mt.MilitaryDischargeType?.Name?.SafeTrim();
                        contract.SkillsAndTraining                     = mt.SkillsAndTraining;
                        contract.IsEligibleForBenefitsYesNoUnknown     = mt.PolarLookupId;
                        contract.IsEligibleForBenefitsYesNoUnknownName = mt.IsEligibleForBenefitsPolarLookup?.Name?.SafeTrim();
                        contract.BenefitsDetails                       = mt.BenefitsDetails;
                    }

                    contract.Notes = mt.Notes;
                }

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.MilitaryTrainingAssessmentSection != null)
                {
                    contract.AssessmentRowVersion     = ia.MilitaryTrainingAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }

            return contract;
        }
    }
}
