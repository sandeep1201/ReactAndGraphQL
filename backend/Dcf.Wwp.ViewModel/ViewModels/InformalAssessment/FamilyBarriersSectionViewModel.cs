using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Learnfare = Dcf.Wwp.Api.Library.Contracts.Cww.Learnfare;
using SocialSecurityStatus = Dcf.Wwp.Api.Library.Contracts.Cww.SocialSecurityStatus;
using Constants = Dcf.Wwp.Model.Interface.Constants;


namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class FamilyBarriersSectionViewModel : BaseInformalAssessmentViewModel
    {
        public FamilyBarriersSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {

        }

        public FamilyBarriersSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        public bool PostData(FamilyBarriersSectionContract contract, string user)
        {
            var part = Participant;
            if (part == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("Family Barriers data is missing.");

            IFamilyBarriersSection fbs;
            IFamilyBarriersAssessmentSection fbas = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (part.InProgressInformalAssessment != null)
            {
                fbas = part.InProgressInformalAssessment.FamilyBarriersAssessmentSection ?? Repo.NewFamilyBarriersAssessmentSection(part.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(fbas);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                fbas.ReviewCompleted = true;
                fbas.ModifiedBy      = AuthUser.Username;
                fbas.ModifiedDate    = updateTime;
            }

            fbs = part.FamilyBarriersSection ?? Repo.NewFamilyBarriersSection(part, user);
            fbs.ModifiedBy   = AuthUser.Username;
            fbs.ModifiedDate = updateTime;

            Repo.StartChangeTracking(fbs);

            var userRowVersion = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            var modDate = DateTime.Now;

            fbs.Notes = contract.Notes;

            // If currently in the process of applying for SSI or SSDI.
            fbs.HasEverAppliedSsi = contract.HasEverAppliedSsi;
            if (fbs.HasEverAppliedSsi.HasValue && fbs.HasEverAppliedSsi.Value)
            {
                fbs.IsCurrentlyApplyingSsi = contract.IsCurrentlyApplyingSsi;
                if (fbs.IsCurrentlyApplyingSsi.HasValue && fbs.IsCurrentlyApplyingSsi.Value)
                {
                    fbs.SsiApplicationStatusId = contract.SsiApplicationStatusId;
                    // We need the type so we can do special logic below based upon which
                    // option was selected.
                    IApplicationStatusType applicationStatusType = Repo.ApplicationStatusTypeById(fbs.SsiApplicationStatusId);

                    // If we already have a detail record, then just update it with whatever we get
                    // from the front end contract.
                    if (fbs.FamilyBarriersSsiApplicationStatusDetail != null)
                    {
                        fbs.FamilyBarriersSsiApplicationStatusDetail.Details = contract.SsiApplicationStatusDetails;
                    }
                    // If we don't have a record, we'll add one if there are details to add.
                    else if (!string.IsNullOrWhiteSpace(contract.SsiApplicationStatusDetails))
                    {
                        fbs.FamilyBarriersSsiApplicationStatusDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                        fbs.FamilyBarriersSsiApplicationStatusDetail.Details = contract.SsiApplicationStatusDetails;
                    }

                    if (applicationStatusType != null)
                    {
                        if (applicationStatusType.ApplicationStatusName.Contains("Applied") ||
                            applicationStatusType.ApplicationStatusName.Contains("Appeal"))
                        {
                            fbs.SsiApplicationDate = contract.SsiApplicationDate.ToDateTimeMonthYear();
                        }
                        else
                        {
                            fbs.SsiApplicationDate = null;
                        }

                        fbs.SsiApplicationIsAnyoneHelping = contract.SsiApplicationIsAnyoneHelping;
                        if (fbs.SsiApplicationIsAnyoneHelping == true)
                        {
                            // If we already have a detail record, then just update it with whatever we get
                            // from the front end contract.
                            if (fbs.FamilyBarriersSsiApplicationDetail != null)
                            {
                                fbs.FamilyBarriersSsiApplicationDetail.Details = contract.SsiApplicationDetails;
                            }
                            // If we don't have a record, we'll add one if there are details to add.
                            else if (!string.IsNullOrWhiteSpace(contract.SsiApplicationDetails))
                            {
                                fbs.FamilyBarriersSsiApplicationDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                                fbs.FamilyBarriersSsiApplicationDetail.Details = contract.SsiApplicationDetails;
                            }

                            fbs.SsiApplicationContactId = contract.SsiApplicationContactId;
                        }
                        else
                        {
                            fbs.Contact = null;
                            fbs.FamilyBarriersSsiApplicationDetail = null;
                        }
                    } // END Application Status not null
                    else
                    {
                        fbs.SsiApplicationDate = null;
                        fbs.SsiApplicationIsAnyoneHelping = null;
                        fbs.FamilyBarriersSsiApplicationDetail = null;
                        fbs.SsiApplicationContactId = null;
                    }
                }   // END Is Currently Applying
                else
                {
                    fbs.ApplicationStatusType = null;
                    fbs.Contact = null;
                    fbs.SsiApplicationIsAnyoneHelping = null;
                    fbs.SsiApplicationDate = null;
                    fbs.FamilyBarriersSsiApplicationStatusDetail = null;
                    fbs.FamilyBarriersAnyoneApplyingForSsiDetail = null;
                }

                // If participant has received SSI or SSDI in the past then include details.
                fbs.HasReceivedPastSsi = contract.HasReceivedPastSsi;
                if (fbs.HasReceivedPastSsi == true)
                {
                    // If we already have a detail record, then just update it with whatever we get
                    // from the front end contract.
                    if (fbs.FamilyBarriersPastSsiDetail != null)
                    {
                        fbs.FamilyBarriersPastSsiDetail.Details = contract.PastSsiDetails;
                    }
                    // If we don't have a record, we'll add one if there are details to add.
                    else if (!string.IsNullOrWhiteSpace(contract.PastSsiDetails))
                    {
                        fbs.FamilyBarriersPastSsiDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                        fbs.FamilyBarriersPastSsiDetail.Details = contract.PastSsiDetails;
                    }

                    if (fbs.FamilyBarriersReasonForPastSsiDetail != null)
                    {
                        fbs.FamilyBarriersReasonForPastSsiDetail.Details = contract.FamilyBarriersReasonForPastSsiDetails;
                    }
                    else if (!string.IsNullOrWhiteSpace(contract.FamilyBarriersReasonForPastSsiDetails))
                    {
                        fbs.FamilyBarriersReasonForPastSsiDetail         = Repo.NewFamilyBarriersDetail(fbs, user);
                        fbs.FamilyBarriersReasonForPastSsiDetail.Details = contract.FamilyBarriersReasonForPastSsiDetails;
                    }
                }
                else
                {
                    fbs.FamilyBarriersPastSsiDetail = null;
                    fbs.FamilyBarriersReasonForPastSsiDetail = null;
                }

                // If participant has ever been denied for SSI or SSDI application process then include details.
                fbs.HasDeniedSsi = contract.HasDeniedSsi;
                if (fbs.HasDeniedSsi == true)
                {
                    fbs.DeniedSsiDate = contract.DeniedSsiDate.ToDateTimeMonthYear();
                    // If we already have a detail record, then just update it with whatever we get
                    // from the front end contract.
                    if (fbs.FamilyBarriersDeniedSsiDetail != null)
                    {
                        fbs.FamilyBarriersDeniedSsiDetail.Details = contract.DeniedSsiDetails;
                    }
                    // If we don't have a record, we'll add one if there are details to add.
                    else if (!string.IsNullOrWhiteSpace(contract.DeniedSsiDetails))
                    {
                        fbs.FamilyBarriersDeniedSsiDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                        fbs.FamilyBarriersDeniedSsiDetail.Details = contract.DeniedSsiDetails;
                    }
                }
                else
                {
                    fbs.DeniedSsiDate = null;
                    fbs.FamilyBarriersDeniedSsiDetail = null;
                }
            }
            else
            {
                fbs.IsCurrentlyApplyingSsi = null;
                fbs.ApplicationStatusType = null;
                fbs.Contact = null;
                fbs.SsiApplicationIsAnyoneHelping = null;
                fbs.SsiApplicationDate = null;
                fbs.FamilyBarriersSsiApplicationStatusDetail = null;
                fbs.FamilyBarriersAnyoneApplyingForSsiDetail = null;
                fbs.HasReceivedPastSsi = null;
                fbs.FamilyBarriersPastSsiDetail = null;
                fbs.FamilyBarriersReasonForPastSsiDetail = null;
                fbs.HasDeniedSsi = null;
                fbs.DeniedSsiDate = null;
                fbs.FamilyBarriersDeniedSsiDetail = null;
            }

            // If participants interested in learning more about the SSI/SSDI application process.
            fbs.IsInterestedInLearningMoreSsi = contract.IsInterestedInLearningMoreSsi;
            if (fbs.IsInterestedInLearningMoreSsi == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersInterestedInLearningMoreSsiDetail != null)
                {
                    fbs.FamilyBarriersInterestedInLearningMoreSsiDetail.Details = contract.InterestedInLearningMoreSsiDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.InterestedInLearningMoreSsiDetails))
                {
                    fbs.FamilyBarriersInterestedInLearningMoreSsiDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersInterestedInLearningMoreSsiDetail.Details = contract.InterestedInLearningMoreSsiDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersInterestedInLearningMoreSsiDetail = null;
            }

            // Has anyone in your family ever applied for SSI or SSDI?
            fbs.HasAnyoneAppliedForSsi = contract.HasAnyoneAppliedForSsi;

            if (fbs.HasAnyoneAppliedForSsi != true)
            {
                fbs.IsAnyoneReceivingSsi = null;
                fbs.FamilyBarriersAnyOneReceivingSsiDetail = null;
                fbs.IsAnyoneApplyingForSsi = null;
                fbs.FamilyBarriersAnyoneApplyingForSsiDetail = null;

                // Also set the contract so we can simplify the code further down.
                contract.IsAnyoneReceivingSsi = null;
                contract.IsAnyoneApplyingForSsi = null;
            }

            fbs.IsAnyoneReceivingSsi = contract.IsAnyoneReceivingSsi;
            if (fbs.IsAnyoneReceivingSsi == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersAnyOneReceivingSsiDetail != null)
                {
                    fbs.FamilyBarriersAnyOneReceivingSsiDetail.Details = contract.AnyoneReceivingSsiDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.AnyoneReceivingSsiDetails))
                {
                    fbs.FamilyBarriersAnyOneReceivingSsiDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersAnyOneReceivingSsiDetail.Details = contract.AnyoneReceivingSsiDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersAnyOneReceivingSsiDetail = null;
            }

            fbs.IsAnyoneApplyingForSsi = contract.IsAnyoneApplyingForSsi;
            if (fbs.IsAnyoneApplyingForSsi == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersAnyoneApplyingForSsiDetail != null)
                {
                    fbs.FamilyBarriersAnyoneApplyingForSsiDetail.Details = contract.AnyoneApplyingForSsiDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.AnyoneApplyingForSsiDetails))
                {
                    fbs.FamilyBarriersAnyoneApplyingForSsiDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersAnyoneApplyingForSsiDetail.Details = contract.AnyoneApplyingForSsiDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersAnyoneApplyingForSsiDetail = null;
            }

            // Notes.
            fbs.Notes = contract.Notes;

            // Family Needs

            #region FamilyMembers repeaters

            // Grab all the family member records from the database which includes the soft deleted item.
            var allFamilyMembers = fbs.FamilyMembers.ToList();
            List<int> familyMemberIds;

            //  caretaking responsibilities for any family members in your household due to health problems or other special needs then list out 
            //individual, your relationship to them, and your caretaking responsibilities.
            fbs.HasCaretakingResponsibilities = contract.HasCaretakingResponsibilities;

            if (fbs.HasCaretakingResponsibilities.HasValue && fbs.HasCaretakingResponsibilities.Value)
            {
                // Now, cleanse the FamilyMembers incoming data.  This means clearing out empty repeater items.
                contract.FamilyMembers = contract.FamilyMembers.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                contract.FamilyMembers.UpdateNewItemsIfSimilarToExisting(allFamilyMembers,
                    FamilyMemberContract.AdoptIfSimilarToModel);
                // Get the Id's of the FamilyMemberContract records that are not new.
                familyMemberIds = (from x in contract.FamilyMembers where x.Id != 0 select x.Id).ToList();
            }
            else
            {
                // The simplest thing to do if the posted contract data indicates there should
                // be no Family members (HasCaretakingResponsibilities  == false) is to just clear out any incoming records.
                // That will allow the normal logic below to just mark any existing records as
                // deleted, even if they were posted in the data contract.
                contract.FamilyMembers = null;

                // We also rely on the preTeenIds and preTeenChildIds to be valid for the logic
                // further down, so we will need up an empty list since there aren't any.
                familyMemberIds = new List<int>();

                // And the logic is that if the answer is No, the question about concerns should
                // be cleared out.  Setting the contract value to NULL will give us the desired
                // effect.
                contract.HasConcernsAboutCaretakingResponsibilities = null;
            }

            // At this point we have the model collections cleaned up and ready to mark the unused
            // items as deleted.
            // Start with getting the active Familymember IDs.
            // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
            // to mark the unused items as deleted.

            //var familyMembersPostedData = familyMemberIds.ToList();
            //allFamilyMembers.MarkUnusedItemsAsDeleted(familyMembersPostedData);

            // Set Delete Reason Ids for Deleted Data.
            if (contract.DeletedFamilyMembers != null)
            {
                foreach (var fmember in contract.DeletedFamilyMembers)
                {
                    var deletedFm = fbs.FamilyMembers.FirstOrDefault(x => x.Id == fmember.Id);
                    if (deletedFm != null)
                        deletedFm.DeleteReasonId = fmember.DeleteReasonId;
                }

            }


            // variable used in looping logic below
            IFamilyMember fm;

            // Now update the database items with the posted model data.

            if (contract.FamilyMembers != null)
            {
                foreach (var fms in contract.FamilyMembers)
                {
                    if (fms.IsNew())
                    {
                        fm = Repo.NewFamilyMember(fbs);
                        fm.ModifiedDate = modDate;
                        fm.ModifiedBy = user;
                    }
                    else
                    {
                        fm = (from x in allFamilyMembers where x.Id == fms.Id select x).SingleOrDefault();
                    }

                    Debug.Assert(fm != null, "IFamilyMember should not be null.");

                    fm.RelationshipId = fms.RelationshipId;
                    // We know if they are in this collection they are this age category.
                    fm.FirstName = fms.FirstName;
                    fm.LastName = fms.LastName;
                    fm.Details = fms.Details;
                    fm.DeleteReasonId = null; // Need to set this in case we've un-soft-deleted an item up above.
                }
            }

            #endregion

            // Do you have concerns that these caretaking responsibilities will make it hard to participate in work actvities
            fbs.HasConcernsAboutCaretakingResponsibilities = contract.HasConcernsAboutCaretakingResponsibilities;
            if (fbs.HasConcernsAboutCaretakingResponsibilities == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail != null)
                {
                    fbs.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail.Details = contract.ConcernsAboutCaretakingResponsibilitiesDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.ConcernsAboutCaretakingResponsibilitiesDetails))
                {
                    fbs.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail.Details = contract.ConcernsAboutCaretakingResponsibilitiesDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail = null;
            }

            // Do any family members in your household engage in risky activities such as excessive use of drugs or alcohol, illegal activity, or gang involvement?
            fbs.DoesHouseholdEngageInRiskyActivities = contract.DoesHouseholdEngageInRiskyActivities;
            if (fbs.DoesHouseholdEngageInRiskyActivities == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersHouseholdEngageRiskyActivitiesDetail != null)
                {
                    fbs.FamilyBarriersHouseholdEngageRiskyActivitiesDetail.Details = contract.HouseholdEngageInRiskyActivitiesDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.HouseholdEngageInRiskyActivitiesDetails))
                {
                    fbs.FamilyBarriersHouseholdEngageRiskyActivitiesDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersHouseholdEngageRiskyActivitiesDetail.Details = contract.HouseholdEngageInRiskyActivitiesDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersHouseholdEngageRiskyActivitiesDetail = null;
            }

            // Any of the children in your household have other behavior problems that will affect your ability to participate in work activities
            fbs.DoChildrenHaveBehaviourProblems = contract.DoChildrenHaveBehaviourProblems;
            if (fbs.DoChildrenHaveBehaviourProblems == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersChildrenHaveBehaviourProblemsDetail != null)
                {
                    fbs.FamilyBarriersChildrenHaveBehaviourProblemsDetail.Details = contract.ChildrenHaveBehaviourProblemsDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.ChildrenHaveBehaviourProblemsDetails))
                {
                    fbs.FamilyBarriersChildrenHaveBehaviourProblemsDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersChildrenHaveBehaviourProblemsDetail.Details = contract.ChildrenHaveBehaviourProblemsDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersChildrenHaveBehaviourProblemsDetail = null;
            }

            // Are any of the children in your Household at risk of suspension or expulsion from school?
            fbs.AreChildrenAtRiskOfSchoolSuspension = contract.AreChildrenAtRiskOfSchoolSuspension;
            if (fbs.AreChildrenAtRiskOfSchoolSuspension == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail != null)
                {
                    fbs.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail.Details = contract.ChildrenAtRiskOfSchoolSuspensionDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.ChildrenAtRiskOfSchoolSuspensionDetails))
                {
                    fbs.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail.Details = contract.ChildrenAtRiskOfSchoolSuspensionDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail = null;
            }

            // Are there any other issues with your family that may affect your ability to participate in work activities?
            fbs.AreAnyFamilyIssuesAffectWork = contract.AreAnyFamilyIssuesAffectWork;
            if (fbs.AreAnyFamilyIssuesAffectWork == true)
            {
                // If we already have a detail record, then just update it with whatever we get
                // from the front end contract.
                if (fbs.FamilyBarriersAnyFamilyIssuesAffectWorkDetail != null)
                {
                    fbs.FamilyBarriersAnyFamilyIssuesAffectWorkDetail.Details = contract.AnyFamilyIssuesAffectWorkDetails;
                }
                // If we don't have a record, we'll add one if there are details to add.
                else if (!string.IsNullOrWhiteSpace(contract.AnyFamilyIssuesAffectWorkDetails))
                {
                    fbs.FamilyBarriersAnyFamilyIssuesAffectWorkDetail = Repo.NewFamilyBarriersDetail(fbs, user);
                    fbs.FamilyBarriersAnyFamilyIssuesAffectWorkDetail.Details = contract.AnyFamilyIssuesAffectWorkDetails;
                }
            }
            else
            {
                fbs.FamilyBarriersAnyFamilyIssuesAffectWorkDetail = null;
            }

            var currentIA = Repo.GetMostRecentAssessment(part);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(fbs, userRowVersion))
                return false;

            if (!Repo.IsRowVersionStillCurrent(fbas, userAssessRowVersion))
                return false;

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.
            if (!Repo.SaveIfChanged(fbs, user))
                Repo.SaveIfChanged(fbas, user);

            return true;
        }

        public static FamilyBarriersSectionContract GetContract(IInformalAssessment ia, IParticipant part, IRepository repo)
        {
            var contract = new FamilyBarriersSectionContract();

            // Since the action needed is not dependent upon any section or assessment section data,
            // we'll add it right away.
            contract.ActionNeeded = ActionNeededViewModel.GetActionNeededContract(part, repo, Constants.ActionNeededPage.FamilyBarriers);

            if (ia != null)
            {
                if (part?.FamilyBarriersSection != null)
                {
                    var fbs = part.FamilyBarriersSection;

                    // Have you ever applied for SSI or SSDI?
                    contract.HasEverAppliedSsi = fbs.HasEverAppliedSsi;
                    if (contract.HasEverAppliedSsi == true)
                    {
                        contract.IsCurrentlyApplyingSsi = fbs.IsCurrentlyApplyingSsi;
                        if (contract.IsCurrentlyApplyingSsi == true)
                        {
                            contract.SsiApplicationStatusId = fbs.SsiApplicationStatusId;
                            if (fbs.ApplicationStatusType != null)
                                contract.SsiApplicationStatusName = fbs.ApplicationStatusType.ApplicationStatusName;
                            contract.SsiApplicationDate = fbs.SsiApplicationDate.ToStringMonthYear();
                            contract.SsiApplicationStatusDetails = fbs.FamilyBarriersSsiApplicationStatusDetail?.Details;
                            contract.SsiApplicationIsAnyoneHelping = fbs.SsiApplicationIsAnyoneHelping;
                            if (contract.SsiApplicationIsAnyoneHelping == true)
                            {
                                contract.SsiApplicationDetails = fbs.FamilyBarriersSsiApplicationDetail?.Details;
                                contract.SsiApplicationContactId = fbs.SsiApplicationContactId;
                            }
                        }
                    }

                    // Have you received SSI or SSDI in the past?
                    contract.HasReceivedPastSsi = fbs.HasReceivedPastSsi;
                    if (contract.HasReceivedPastSsi == true)
                    {
                        contract.PastSsiDetails = fbs.FamilyBarriersPastSsiDetail?.Details;
                        contract.FamilyBarriersReasonForPastSsiDetails = fbs.FamilyBarriersReasonForPastSsiDetail?.Details;
                    }

                    // Have you ever been denied for SSI or SSDI?
                    contract.HasDeniedSsi = fbs.HasDeniedSsi;
                    if (contract.HasDeniedSsi == true)
                    {
                        contract.DeniedSsiDate = fbs.DeniedSsiDate.ToStringMonthYear();
                        contract.DeniedSsiDetails = fbs.FamilyBarriersDeniedSsiDetail?.Details;
                    }

                    // Are you interested in learning more about the SSI/SSDI application process?
                    contract.IsInterestedInLearningMoreSsi = fbs.IsInterestedInLearningMoreSsi;
                    if (contract.IsInterestedInLearningMoreSsi == true)
                    {
                        contract.InterestedInLearningMoreSsiDetails = fbs.FamilyBarriersInterestedInLearningMoreSsiDetail?.Details;
                    }
                    // Is anyone in your family receiving SSI or SSDI?
                    contract.IsAnyoneReceivingSsi = fbs.IsAnyoneReceivingSsi;
                    if (contract.IsAnyoneReceivingSsi == true)
                    {
                        contract.AnyoneReceivingSsiDetails = fbs.FamilyBarriersAnyOneReceivingSsiDetail?.Details;
                    }

                    // Is anyone in your family in the process of applying for SSI or SSDI?
                    contract.HasAnyoneAppliedForSsi = fbs.HasAnyoneAppliedForSsi;
                    if (contract.HasAnyoneAppliedForSsi == true)
                    {
                        contract.IsAnyoneReceivingSsi = fbs.IsAnyoneReceivingSsi;
                        if (contract.IsAnyoneReceivingSsi == true)
                        {
                            contract.AnyoneReceivingSsiDetails = fbs.FamilyBarriersAnyOneReceivingSsiDetail?.Details;
                        }
                        contract.IsAnyoneApplyingForSsi = fbs.IsAnyoneApplyingForSsi;
                        if (contract.IsAnyoneApplyingForSsi == true)
                        {
                            contract.AnyoneApplyingForSsiDetails = fbs.FamilyBarriersAnyoneApplyingForSsiDetail?.Details;
                        }

                    }

                    contract.FamilyMembers = new List<FamilyMemberContract>();

                    // Do you have caretaking responsibilities for any family members in your household due to health problems or other special needs?

                    contract.HasCaretakingResponsibilities = fbs.HasCaretakingResponsibilities;
                    if (contract.HasCaretakingResponsibilities.HasValue && contract.HasCaretakingResponsibilities.Value)
                    {
                        // family needs
                        foreach (var fm in fbs.NonDeletedFamilyMembers)
                        {
                            contract.FamilyMembers.Add(new FamilyMemberContract()
                            {
                                Id = fm.Id,
                                FirstName = fm.FirstName,
                                LastName = fm.LastName,
                                RelationshipId = fm.RelationshipId,
                                RelationshipName = fm.Relationship?.RelationName,
                                Details = fm.Details
                            });
                        }
                    }

                    // Do you have concerns that these caretaking responsibilities will make it hard to participate in work activities?
                    contract.HasConcernsAboutCaretakingResponsibilities = fbs.HasConcernsAboutCaretakingResponsibilities;
                    if (contract.HasConcernsAboutCaretakingResponsibilities == true)
                    {
                        contract.ConcernsAboutCaretakingResponsibilitiesDetails = fbs.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail?.Details;
                    }

                    // Do any family members in your household engage in risky activities such as excessive use of drugs or alcohol, illegal activity, or gang involvement?
                    contract.DoesHouseholdEngageInRiskyActivities = fbs.DoesHouseholdEngageInRiskyActivities;
                    if (contract.DoesHouseholdEngageInRiskyActivities == true)
                    {
                        contract.HouseholdEngageInRiskyActivitiesDetails = fbs.FamilyBarriersHouseholdEngageRiskyActivitiesDetail?.Details;
                    }

                    // Do any of the children in your household have other behavior problems that will affect your ability to participate in work activities?
                    contract.DoChildrenHaveBehaviourProblems = fbs.DoChildrenHaveBehaviourProblems;
                    if (contract.DoChildrenHaveBehaviourProblems == true)
                    {
                        contract.ChildrenHaveBehaviourProblemsDetails = fbs.FamilyBarriersChildrenHaveBehaviourProblemsDetail?.Details;
                    }

                    // Are any of the children in your Household at risk of suspension or expulsion from school?
                    contract.AreChildrenAtRiskOfSchoolSuspension = fbs.AreChildrenAtRiskOfSchoolSuspension;
                    if (contract.AreChildrenAtRiskOfSchoolSuspension == true)
                    {
                        contract.ChildrenAtRiskOfSchoolSuspensionDetails = fbs.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail?.Details;
                    }

                    // Are there any other issues with your family that may affect your ability to participate in work activities?
                    contract.AreAnyFamilyIssuesAffectWork = fbs.AreAnyFamilyIssuesAffectWork;
                    if (contract.AreAnyFamilyIssuesAffectWork == true)
                    {
                        contract.AnyFamilyIssuesAffectWorkDetails = fbs.FamilyBarriersAnyFamilyIssuesAffectWorkDetail?.Details;
                    }

                    contract.Notes = fbs.Notes;
                    // Standard modified by/rowversion stuff
                    contract.RowVersion = fbs.RowVersion;
                    contract.ModifiedBy = fbs.ModifiedBy;
                    contract.ModifiedDate = fbs.ModifiedDate;
                }

                // CWW Info for Cww Learnfare is being commented out based on a CR
                contract.CwwLearnfare = new List<Learnfare>();
                var learfares = repo.CwwLearnfare(part.PinNumber.ToString());

                foreach (var lf in learfares)
                {
                    contract.CwwLearnfare.Add(new Learnfare() { FirstName = lf.FirstName, LastName = lf.LastName, MiddleInitial = lf.Middle, BirthDate = lf.BirthDate.ToStringMonthDayYear(), LearnFareStatus = lf.LearnfareStatus });
                }

                //// CWW Info for CWW Social Security Income
                //contract.CwwSocialSecurityStatus = new List<SocialSecurityStatus>();
                //var socialSecurityStatuses = repo.CwwSocialSecurityStatus(part.PinNumber.ToString());

                //foreach (var sss in socialSecurityStatuses)
                //{
                //    contract.CwwSocialSecurityStatus.Add(new SocialSecurityStatus()
                //    {
                //        Participant = sss.Participant,
                //        FirstName = sss.FirstName,
                //        Middle = sss.Middle,
                //        LastName = sss.LastName,
                //        Dob = sss.Dob,
                //        Relationship = sss.Relationship,
                //        Age = sss.Age,
                //        FedSsi = sss.FedSsi,
                //        StateSsi = sss.StateSsi,
                //        Ssa = sss.Ssa
                //    });
                //}

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.FamilyBarriersAssessmentSection != null)
                {
                    contract.AssessmentRowVersion = ia.FamilyBarriersAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }

            return contract;
        }
    }
}
