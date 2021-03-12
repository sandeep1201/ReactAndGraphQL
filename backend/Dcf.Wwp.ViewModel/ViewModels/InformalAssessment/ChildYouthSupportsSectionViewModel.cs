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
using Dcf.Wwp.Api.Library.Utils;
using Constants = Dcf.Wwp.Model.Interface.Constants;


namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class ChildYouthSupportsSectionViewModel : BaseInformalAssessmentViewModel
    {
        public ChildYouthSupportsSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public ChildYouthSupportsSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        public bool PostData(ChildYouthSupportsSectionContract contract, string user)
        {
            var ia   = InformalAssessment;
            var part = Participant;

            if (part == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("Child Youth Supports data is missing.");

            if (ia == null)
                throw new InvalidOperationException("InformalAssessment record is missing.");

            var updateTime = DateTime.Now;

            try
            {
                //var modDate = DateTime.Now;
                var ageCat12Under = Repo.AgeCategoryByName(Constants.AgeCategory.AgeRangeTwelveAndUnder);
                var ageCat13To18  = Repo.AgeCategoryByName(Constants.AgeCategory.AgeRangeThirteenToEighteen);

                IChildYouthSection cys = part.ChildYouthSection ?? Repo.NewChildYouthSection(part.Id, user);
                cys.ModifiedBy   = AuthUser.Username;
                cys.ModifiedDate = updateTime;
                Repo.StartChangeTracking(cys);

                IChildYouthSupportsAssessmentSection cysas = ia.ChildYouthSupportsAssessmentSection ?? Repo.NewChildYouthSupportsAssessmentSection(ia, user);
                cysas.ModifiedBy   = AuthUser.Username;
                cysas.ModifiedDate = updateTime;
                Repo.StartChangeTracking(cysas);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                cysas.ReviewCompleted = true;

                var userRowVersion    = contract.RowVersion;
                var userAssRowVersion = contract.AssessmentRowVersion;

                #region Child and Teen repeaters

                // Grab all the child records from the database which includes the soft deleted item.
                // This list contains all children for both PreTeens and Teens showing up in the UI.
                var allChildYouSectionChilds = cys.ChildYouthSectionChilds.ToList();

                // We will process the two lists of children (12 and under or 13+) in two chunks, but
                // since we combine them into one list we need some other variables.
                List<int> preTeenIds;
                List<int> teenIds;

                List<int> preTeenChildIds;
                List<int> teenChildIds;

                cys.HasChildren12OrUnder = contract.HasChildren;

                if (cys.HasChildren12OrUnder.HasValue && cys.HasChildren12OrUnder.Value)
                {
                    // First, cleanse the incoming PreTeen data.  This means clearing out empty repeater items.
                    contract.Children = contract.Children.WithoutEmpties();
                    // Next map any new items that are similar to existing/deleted items.
                    contract.Children.UpdateNewItemsIfSimilarToExisting(allChildYouSectionChilds, ChildCareContract.AdoptIfSimilarToModel);

                    // Get the Id's of the ChildCareContract records that are not new.
                    preTeenIds = (from x in contract.Children where x.Id != 0 select x.Id).ToList();

                    // Now we need to mark the Child records not being used as deleted.
                    preTeenChildIds = (from x in contract.Children where x.ChildId != 0 select x.ChildId).ToList();
                }
                else
                {
                    // The simplest thing to do if the posted contract data indicates there should
                    // be no children (HasChildren == false) is to just clear out any incoming records.
                    // That will allow the normal logic below to just mark any existing records as
                    // deleted, even if they were posted in the data contract.
                    contract.Children = null;

                    // We also rely on the preTeenIds and preTeenChildIds to be valid for the logic
                    // further down, so we will need up an empty list since there aren't any.
                    preTeenIds      = new List<int>();
                    preTeenChildIds = new List<int>();
                }

                cys.HasChildrenOver12WithDisabilityInNeedOfChildCare = contract.HasTeensWithDisabilityInNeedOfChildCare;

                if (cys.HasChildrenOver12WithDisabilityInNeedOfChildCare.HasValue && cys.HasChildrenOver12WithDisabilityInNeedOfChildCare.Value)
                {
                    // Now, cleanse the Teens incoming data.  This means clearing out empty repeater items.
                    contract.Teens = contract.Teens.WithoutEmpties();
                    // Next map any new items that are similar to existing/deleted items.
                    contract.Teens.UpdateNewItemsIfSimilarToExisting(allChildYouSectionChilds, TeenCareContract.AdoptIfSimilarToModel);

                    // Get the Id's of the TeenCareContract records that are not new.
                    teenIds = (from x in contract.Teens where x.Id != 0 select x.Id).ToList();

                    // Now we need to mark the Child records not being used as deleted.
                    teenChildIds = (from x in contract.Teens where x.ChildId != 0 select x.ChildId).ToList();
                }
                else
                {
                    // The simplest thing to do if the posted contract data indicates there should
                    // be no children (HasChildren == false) is to just clear out any incoming records.
                    // That will allow the normal logic below to just mark any existing records as
                    // deleted, even if they were posted in the data contract.
                    contract.Teens = null;

                    // We also rely on the preTeenIds and preTeenChildIds to be valid for the logic
                    // further down, so we will need up an empty list since there aren't any.
                    teenIds      = new List<int>();
                    teenChildIds = new List<int>();
                }

                // At this point we have the model collections cleaned up and ready to mark the unused
                // items as deleted.  We need to join the two lists since they all feed into the same
                // ChildYoutSectionChild and Child tables.
                //
                // This is more complex than normal repeaters since we have two collections and we have
                // two levels of IDs that need to be checked.
                //
                // Start with getting the active ChildYoutSectionChild IDs.

                // Join the two lists of ChildCareContract and TeenCareContract Ids.  Using Union will
                // result in elements from both input sequences, excluding duplicates.
                var cyscIdsInPostedData = preTeenIds.Union(teenIds).ToList();

                // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
                // to mark the unused items as deleted.
                //  allChildYouSectionChilds.MarkUnusedItemsAsDeleted(cyscIdsInPostedData);
                if (contract.DeletedChildren != null)
                {
                    foreach (var dc in contract.DeletedChildren)
                    {
                        var child = allChildYouSectionChilds.FirstOrDefault(x => x.Id == dc.Id);
                        if (child != null)
                            child.DeleteReasonId = dc.DeleteReasonId;
                    }
                }

                if (contract.DeletedTeens != null)
                {
                    foreach (var dt in contract.DeletedTeens)
                    {
                        var teen = allChildYouSectionChilds.FirstOrDefault(x => x.Id == dt.Id);
                        if (teen != null)
                            teen.DeleteReasonId = dt.DeleteReasonId;
                    }
                }


                // Now we need to mark the Child records not being used as deleted.
                var childIds = preTeenChildIds.Union(teenChildIds).ToList();

                // Get all the child Id records, including deleted records.
                var allChildrenForParticipant = (from x in allChildYouSectionChilds select x.Child).ToList();

                // Mark the unused items as deleted.
                allChildrenForParticipant.MarkUnusedItemsAsDeleted(childIds);

                // variable used in looping logic below
                IChildYouthSectionChild cysc;

                // Now update the database items with the posted model data.
                // Pre-Teen (Children 12 & under first)
                if (contract.Children != null)
                {
                    foreach (var cc in contract.Children)
                    {
                        if (cc.IsNew())
                        {
                            cysc              = Repo.NewChildYouthSectionChild(cys);
                            cysc.ModifiedDate = updateTime;
                            cysc.ModifiedBy   = user;
                        }
                        else
                        {
                            cysc = (from x in allChildYouSectionChilds where x.Id == cc.Id select x).SingleOrDefault();
                        }

                        Debug.Assert(cysc != null, "IChildYouthSectionChild should not be null.");

                        cysc.CareArrangementId = cc.CareArrangementId;
                        // We know if they are in this collection they are this age category.
                        cysc.AgeCategoryId  = ageCat12Under;
                        cysc.IsSpecialNeeds = cc.IsSpecialNeeds;
                        cysc.Details        = cc.Details;
                        cysc.DeleteReason   = null; // Need to set this in case we've un-soft-deleted an item up above.
                        cysc.DeleteReasonId = null;

                        // We may need to create a Child record in the database.
                        if (cc.ChildId == 0)
                        {
                            // Associating it this way will take care of the relationship in the database as
                            // in the foreign keys.
                            cysc.Child              = Repo.NewChild();
                            cysc.Child.ModifiedDate = updateTime;
                            cysc.Child.ModifiedBy   = user;
                        }

                        // We will always set the values of the child (even if they are empty).
                        cysc.Child.DateOfBirth = cc.DateOfBirth.ToDateTimeMonthDayYear();
                        cysc.Child.FirstName   = cc.FirstName;
                        cysc.Child.LastName    = cc.LastName;
                        cysc.Child.IsDeleted   = false; // Need to set this in case we've un-soft-deleted an item up above.
                    }
                }

                // Special Needs Teens
                if (contract.Teens != null)
                {
                    foreach (var tcc in contract.Teens)
                    {
                        if (tcc.IsNew())
                        {
                            cysc              = Repo.NewChildYouthSectionChild(cys);
                            cysc.ModifiedDate = updateTime;
                            cysc.ModifiedBy   = user;
                        }
                        else
                        {
                            cysc = (from x in allChildYouSectionChilds where x.Id == tcc.Id select x).SingleOrDefault();
                        }

                        Debug.Assert(cysc != null, "IChildYouthSectionChild should not be null.");

                        cysc.CareArrangementId = null;
                        // We know if they are in this collection they are this age category.
                        cysc.AgeCategoryId  = ageCat13To18;
                        cysc.IsSpecialNeeds = true; // They must be special needs to be in this group.
                        cysc.Details        = tcc.Details;
                        cysc.DeleteReason   = null; // Need to set this in case we've un-soft-deleted an item up above.
                        cysc.DeleteReasonId = null;

                        // We may need to create a Child record in the database.
                        if (tcc.ChildId == 0)
                        {
                            // Associating it this way will take care of the relationship in the database as
                            // in the foreign keys.
                            cysc.Child              = Repo.NewChild();
                            cysc.Child.ModifiedDate = updateTime;
                            cysc.Child.ModifiedBy   = user;
                        }

                        // We will always set the values of the child (even if they are empty).
                        cysc.Child.DateOfBirth = tcc.DateOfBirth.ToDateTimeMonthDayYear();
                        cysc.Child.FirstName   = tcc.FirstName;
                        cysc.Child.LastName    = tcc.LastName;
                        cysc.Child.IsDeleted   = false; // Need to set this in case we've un-soft-deleted an item up above.
                    }
                }

                #endregion Child and Teen repeaters

                var hasSpecialNeeds = contract.Children?.Any(i => i.IsSpecialNeeds == true) ?? false;

                if (hasSpecialNeeds || contract.HasTeensWithDisabilityInNeedOfChildCare == true)
                {
                    cys.IsSpecialNeedsProgramming      = contract.IsSpecialNeedsProgramming;
                    cys.SpecialNeedsProgrammingDetails = cys.IsSpecialNeedsProgramming == true ? contract.SpecialNeedsProgrammingDetails : null;
                }
                else
                {
                    cys.IsSpecialNeedsProgramming      = null;
                    cys.SpecialNeedsProgrammingDetails = null;
                }

                cys.HasWicBenefits                 = contract.HasWicBenefits;
                cys.IsInHeadStart                  = contract.IsInHeadStart;
                cys.IsInAfterSchoolOrSummerProgram = contract.IsInAfterSchoolOrSummerProgram;
                cys.AfterSchoolProgramDetails      = contract.AfterSchoolOrSummerProgramNotes;
                cys.IsInMentoringProgram           = contract.IsInMentoringProgram;
                cys.MentoringProgramDetails        = contract.MentoringProgramNotes;

                // Update the database object per the posted model data and also apply the pertinent
                // business logic to clean up.
                if (!contract.HasChildUnder5)
                {
                    cys.HasWicBenefits = null;
                    cys.IsInHeadStart  = null;
                }


                if (contract.HasChild5OrOver)
                {
                    cys.IsInAfterSchoolOrSummerProgram = contract.IsInAfterSchoolOrSummerProgram;
                    cys.AfterSchoolProgramDetails      = contract.AfterSchoolOrSummerProgramNotes;
                    cys.IsInMentoringProgram           = contract.IsInMentoringProgram;
                    cys.MentoringProgramDetails        = contract.MentoringProgramNotes;
                }
                else
                {
                    cys.IsInAfterSchoolOrSummerProgram = null;
                    cys.AfterSchoolProgramDetails      = null;
                    cys.IsInMentoringProgram           = null;
                    cys.MentoringProgramDetails        = null;
                }

                // Deleting the details when the posted model has the IsInAfterSchoolOrSummerProgram and IsInMentoringProgram has a value false
                if (contract.IsInAfterSchoolOrSummerProgram == false)
                {
                    cys.AfterSchoolProgramDetails = null;
                }

                if (contract.IsInMentoringProgram == false)
                {
                    cys.MentoringProgramDetails = null;
                }

                cys.HasChildWelfareWorker = contract.HasChildWelfareWorkerId;

                if (contract.HasChildWelfareWorkerId != null && contract.HasChildWelfareWorkerId == 1)
                {
                    cys.ChildWelfareWorkerChildren           = contract.ChildWelfareWorkerChildren;
                    cys.ChildWelfareWorkerPlanOrRequirements = contract.ChildWelfareWorkerPlan;
                    cys.ChildWelfareContactId                = contract.ChildWelfareWorkerContactId;
                }
                else
                {
                    cys.ChildWelfareWorkerChildren           = null;
                    cys.ChildWelfareWorkerPlanOrRequirements = null;
                    cys.ChildWelfareContactId                = null;
                }

                if (part.AgeInYears.HasValue && part.AgeInYears.Value < 26)
                {
                    cys.DidOrWillAgeOutOfFosterCare = contract.DidOrWillAgeOutOfFosterCare;
                    cys.FosterCareDetails           = contract.FosterCareNotes;
                }
                else
                {
                    cys.DidOrWillAgeOutOfFosterCare = null;
                    cys.FosterCareDetails           = null;
                }

                if (part.ChildYouthSection.DidOrWillAgeOutOfFosterCare == false)
                {
                    cys.FosterCareDetails = null;
                }

                cys.HasFutureChildCareNeed = contract.HasFutureChildCareChanges;

                if (cys.HasFutureChildCareNeed.HasValue && cys.HasFutureChildCareNeed.Value)
                    cys.FutureChildCareNeedNotes = contract.FutureChildCareChangesNotes;
                else
                    cys.FutureChildCareNeedNotes = null;

                cys.Notes = contract.Notes;

                // Action Needed is an app and saves itself.

                var currentIA = Repo.GetMostRecentAssessment(part);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;

                // Do a concurrency check.
                if (!Repo.IsRowVersionStillCurrent(cys, userRowVersion))
                    return false;

                if (!Repo.IsRowVersionStillCurrent(cysas, userAssRowVersion))
                    return false;

                // If the first save completes, it actually has already saved the ChildYouthSupportsAssessmentSection
                // object as well since they are on the save repository context.  But if the ChildYouthSection didn't
                // need saving, we still need to SaveIfChangd on the ChildYouthSupportsAssessmentSection.
                if (!Repo.SaveIfChanged(cys,  user))
                    Repo.SaveIfChanged(cysas, user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public static ChildYouthSupportsSectionContract GetContract(IInformalAssessment ia, IParticipant part, IRepository repo)
        {
            var contract = new ChildYouthSupportsSectionContract();

            // Do a quick sanity check to be sure the IA is not null.  If it
            // is we'll send back a completely empty section.
            if (ia == null || part == null) return contract;

            if (part.ChildYouthSection != null)
            {
                var cys = part.ChildYouthSection;
                contract.HasChildren = cys.HasChildren12OrUnder;

                if (contract.HasChildren.HasValue && contract.HasChildren.Value)
                {
                    contract.Children = GetChildren(cys); //refactored
                }

                contract.HasTeensWithDisabilityInNeedOfChildCare = cys.HasChildrenOver12WithDisabilityInNeedOfChildCare;

                if (contract.HasTeensWithDisabilityInNeedOfChildCare.HasValue && contract.HasTeensWithDisabilityInNeedOfChildCare.Value)
                {
                    contract.Teens = GetTeens(cys); //refactored - moved below
                }

                contract.IsSpecialNeedsProgramming      = cys.IsSpecialNeedsProgramming;
                contract.SpecialNeedsProgrammingDetails = cys.SpecialNeedsProgrammingDetails;

                contract.HasWicBenefits                  = cys.HasWicBenefits;
                contract.IsInHeadStart                   = cys.IsInHeadStart;
                contract.IsInAfterSchoolOrSummerProgram  = cys.IsInAfterSchoolOrSummerProgram;
                contract.AfterSchoolOrSummerProgramNotes = cys.AfterSchoolProgramDetails;
                contract.IsInMentoringProgram            = cys.IsInMentoringProgram;
                contract.MentoringProgramNotes           = cys.MentoringProgramDetails;
                contract.HasChildWelfareWorkerId         = cys.HasChildWelfareWorker;
                contract.HasChildWelfareWorkerName       = cys.YesNoUnknownLookup?.Name;
                contract.ChildWelfareWorkerChildren      = cys.ChildWelfareWorkerChildren;
                contract.ChildWelfareWorkerPlan          = cys.ChildWelfareWorkerPlanOrRequirements;
                contract.ChildWelfareWorkerContactId     = cys.ChildWelfareContactId;
                contract.DidOrWillAgeOutOfFosterCare     = cys.DidOrWillAgeOutOfFosterCare;
                contract.FosterCareNotes                 = cys.FosterCareDetails;
                contract.HasFutureChildCareChanges       = cys.HasFutureChildCareNeed;
                contract.FutureChildCareChangesNotes     = cys.FutureChildCareNeedNotes;
                contract.Notes                           = cys.Notes;

                // Standard modified by/rowversion stuff
                contract.RowVersion   = cys.RowVersion;
                contract.ModifiedBy   = cys.ModifiedBy;
                contract.ModifiedDate = cys.ModifiedDate;
            }

            if (ia.ChildYouthSupportsAssessmentSection != null)
            {
                contract.IsSubmittedViaDriverFlow = true;

                // NOTE: 9/20/2017 this was commented out below becuase we the assessment tables are really
                // only now tracking the driver flow so we don't need to update the mod date, etc. if all
                // they are doing is updating the ReviewCompleted flag.

                // Make a call to the helper method in the base class to set the assessment row version and update
                // modified stuff if needed.
                UpdateRowVersionAndModifiedIfAssessmentMoreRecent(contract, part.ChildYouthSection, ia.ChildYouthSupportsAssessmentSection);
            }

            contract.ActionNeeded = ActionNeededViewModel.GetActionNeededContract(part, repo, Constants.ActionNeededPage.ChildAndYouthSupports);

            // CWW info refactored
            contract.CwwChildren = CwwHelper.GetCwwChildren(repo, part);

            // CWW Child Care Eligibility
            if (contract.CwwChildren.Count > 0)
            {
                contract.CwwEligibility = CwwHelper.GetChildCareEligibility(repo, part);
            }

            return contract;
        }

        private static List<ChildCareContract> GetChildren(IChildYouthSection cys)
        {
            var children = cys.PreTeens.Select(cysc => new ChildCareContract
                                                       {
                                                           Id                  = cysc.Id,
                                                           ChildId             = cysc.ChildId ?? 0,
                                                           CareArrangementId   = cysc.CareArrangementId,
                                                           CareArrangementName = cysc.ChildCareArrangement?.Name,
                                                           DateOfBirth         = cysc.Child?.DateOfBirth.ToStringMonthDayYear(),
                                                           IsSpecialNeeds      = cysc.IsSpecialNeeds,
                                                           Details             = cysc.Details,
                                                           FirstName           = cysc.Child?.FirstName,
                                                           LastName            = cysc.Child?.LastName
                                                       })
                              .ToList();

            return children;
        }

        private static List<TeenCareContract> GetTeens(IChildYouthSection cys)
        {
            var teens = cys.Teens.Select(cysc => new TeenCareContract
                                                 {
                                                     Id          = cysc.Id,
                                                     ChildId     = cysc.ChildId ?? 0,
                                                     DateOfBirth = cysc.Child.DateOfBirth.ToStringMonthDayYear(),
                                                     Details     = cysc.Details,
                                                     FirstName   = cysc.Child.FirstName,
                                                     LastName    = cysc.Child.LastName
                                                 })
                           .ToList();

            return teens;
        }


        //TODO : remove below section while cleaning up.. old code
        /*
        // CWW Info
        contract.CwwChildren = new List<Child>();
        var children = repo.CwwCurrentChildren(part.PinNumber.ToString());

        foreach (var chld in children)
        {
            contract.CwwChildren.Add(new Child() { Age = chld.Age, BirthDate = chld.BirthDate?.ToShortDateString(), FirstName = chld.FirstName, Gender = chld.Gender, LastName = chld.LastName, MiddleInitial = chld.Middle, Relationship = chld.Relationship });
        }*/

        /* if (contract.CwwChildren.Count > 0)
        {
            var ccEligiblity = repo.CwwChildCareEligibiltyStatus(part.CaseNumber?.ToString());
            if (ccEligiblity != null)
            {
                contract.CwwEligibility = new ChildCareEligibility
                {
                    EligibilityStatus = ccEligiblity.EligibilityStatus,
                    ReasonCode = ccEligiblity.ReasonCode,
                    Description = ccEligiblity.DescriptionText
                };
            }
        }*/
    }
}
