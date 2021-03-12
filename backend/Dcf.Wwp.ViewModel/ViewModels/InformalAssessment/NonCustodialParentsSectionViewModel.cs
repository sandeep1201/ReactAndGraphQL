using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Constants = Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class NonCustodialParentsSectionViewModel : BaseInformalAssessmentViewModel
    {
        public NonCustodialParentsSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public NonCustodialParentAssessmentContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        /*
         *  NOTES on saving NonCustodialParentAssessmentContract
         *  
         *  The NonCustodialParentAssessmentContract contains some participant based data,
         *  a repeater of Caretakers and for each Caretaker a repeater of children.  Because
         *  of the Unknown nature of Caretakers, we do not attempt to restore soft deleted
         *  repeater records.  This makes the handling of these repeater items different
         *  than other repeaters (up to this point).
         */

        public bool PostData(NonCustodialParentAssessmentContract model, string user)
        {
            var ia   = InformalAssessment;
            var part = Participant;

            if (part == null)
                throw new InvalidOperationException("PIN not valid.");

            if (model == null)
                throw new InvalidOperationException("Non Custodial Parent data is missing.");

            if (ia == null)
                throw new InvalidOperationException("InformalAssessment record is missing.");

            var updateTime = DateTime.Now;

            try
            {
                var modDate = DateTime.Now;

                INonCustodialParentsSection ncps = part.NonCustodialParentsSection ?? Repo.NewNonCustodialParentsSection(part.Id, user);
                ncps.ModifiedBy   = AuthUser.Username;
                ncps.ModifiedDate = updateTime;
                Repo.StartChangeTracking(ncps);

                INonCustodialParentsAssessmentSection ncpas = ia.NonCustodialParentsAssessmentSection ?? Repo.NewNonCustodialParentsAssessmentSection(ia, user);
                Repo.StartChangeTracking(ncpas);

                var userRowVersion    = model.RowVersion;
                var userAssRowVersion = model.AssessmentRowVersion;

                ncps.HasChildren = model.HasChildren;

                // We need an indicator if there are any child support orders as if there aren't then
                // we'll need to clear out some variables.
                bool hadAnyChildSupportOrders = false;

                // Grab all the caretaker records from the database which includes the soft deleted item.
                var       allCaretakers = ncps.AllNonCustodialCaretakers.ToList();
                List<int> caretakerIds;

                if (model.HasChildren.HasValue && model.HasChildren.Value)
                {
                    // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
                    model.NonCustodialCaretakers = model.NonCustodialCaretakers.WithoutEmpties();

                    // NOPE: We don't do any restore process for existing/deleted items.
                    // Next map any new items that are similar to existing/deleted items.
                    //model.NonCustodialCaretakers.UpdateNewItemsIfSimilarToExisting(allCaretakers, NonCustodialCaretakerContract.AdoptIfSimilarToModel);

                    // Get the Id's of the NonCustodialCaretakers that are not new.
                    caretakerIds = (from x in model.NonCustodialCaretakers where x.Id != 0 select x.Id).ToList();

                    ncps.IsInterestedInReferralServices = model.IsInterestedInReferralServices;

                    if (model.IsInterestedInReferralServices.HasValue && model.IsInterestedInReferralServices.Value)
                        ncps.InterestedInReferralServicesDetails = model.InterestedInReferralServicesDetails;
                    else
                        ncps.InterestedInReferralServicesDetails = String.Empty;
                }
                else
                {
                    // The simplest thing to do if the posted contract data indicates there should
                    // be no children (HasChildren == false) is to just clear out any incoming records.
                    // That will allow the normal logic below to just mark any existing records as
                    // deleted, even if they were posted in the data contract.
                    model.NonCustodialCaretakers = null;

                    // Set the repeater IDs to an empty list since there aren't any.  The effect of
                    // this empty list of active IDs is that all the existing Caretaker objects will
                    // be marked as deleted.
                    caretakerIds = new List<int>();

                    // Clean up items that may have also been set regardless of the number of repeater items.
                    ncps.IsInterestedInReferralServices      = null;
                    ncps.InterestedInReferralServicesDetails = String.Empty;
                }

                // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
                // to mark the unused items as deleted.
                // allCaretakers.MarkUnusedItemsAsDeleted(caretakerIds);

                // Set Delete Reason Ids for deleted Data.
                if (model.DeletedNonCustodialCaretakers != null)
                {
                    foreach (var item in model.DeletedNonCustodialCaretakers)
                    {
                        var deletedItem = ncps.NonCustodialCaretakers.FirstOrDefault(x => x.Id == item.Id);

                        if (deletedItem != null)
                            deletedItem.DeleteReasonId = item.DeleteReasonId;
                    }
                }

                // Now update the database items with the posted model data.
                if (model.NonCustodialCaretakers != null)
                {
                    foreach (var m in model.NonCustodialCaretakers)
                    {
                        INonCustodialCaretaker ncc;

                        if (m.IsNew())
                        {
                            ncc              = Repo.NewNonCustodialCaretaker(ncps, user);
                            ncc.ModifiedDate = modDate;
                            ncc.ModifiedBy   = user;
                        }
                        else
                        {
                            ncc = (from x in allCaretakers where x.Id == m.Id select x).SingleOrDefault();
                        }

                        Debug.Assert(ncc != null, "INonCustodialCaretaker should not be null.");

                        ncc.IsFirstNameUnknown               = m.IsFirstNameUnknown.HasValue && m.IsFirstNameUnknown.Value;
                        ncc.FirstName                        = ncc.IsFirstNameUnknown ? null : m.FirstName;
                        ncc.IsLastNameUnknown                = m.IsLastNameUnknown.HasValue && m.IsLastNameUnknown.Value;
                        ncc.LastName                         = ncc.IsLastNameUnknown ? null : m.LastName;
                        ncc.NonCustodialParentRelationshipId = m.NonCustodialParentRelationshipId;
                        ncc.RelationshipDetails              = m.RelationshipDetails;

                        // If the Relationship is Unknown and the name is Unknown, then the questions should be cleared out.
                        if (ncc.IsFirstNameUnknown &&
                            ncc.IsLastNameUnknown  &&
                            (m.NonCustodialParentRelationshipId.HasValue &&
                             m.NonCustodialParentRelationshipId.Value ==
                             Constants.NonCustodialParentRelationship.UnknownId))
                        {
                            ncc.ContactIntervalId                       = null;
                            ncc.ContactIntervalDetails                  = null;
                            ncc.IsRelationshipChangeRequested           = null;
                            ncc.RelationshipChangeRequestedDetails      = null;
                            ncc.IsInterestedInRelationshipReferral      = null;
                            ncc.InterestedInRelationshipReferralDetails = null;
                        }
                        else
                        {
                            ncc.ContactIntervalId                  = m.ContactIntervalId;
                            ncc.ContactIntervalDetails             = m.ContactIntervalDetails;
                            ncc.IsRelationshipChangeRequested      = m.IsRelationshipChangeRequested;
                            ncc.RelationshipChangeRequestedDetails = m.RelationshipChangeRequestedDetails;
                        }

                        if (m.IsRelationshipChangeRequested.HasValue && m.IsRelationshipChangeRequested.Value)
                        {
                            ncc.IsInterestedInRelationshipReferral      = m.IsInterestedInRelationshipReferral;
                            ncc.InterestedInRelationshipReferralDetails = m.InterestedInRelationshipReferralDetails;
                        }
                        else
                        {
                            ncc.IsInterestedInRelationshipReferral      = null;
                            ncc.InterestedInRelationshipReferralDetails = null;
                        }

                        var hadChildSupportOrder = ProcessCaretakerChildren(ncc, m.NonCustodialChilds, m.DeletedNonCustodialChilds, user);

                        // Flip the indicator only to true if any of thi children had a child support order.
                        if (!hadAnyChildSupportOrders)
                            hadAnyChildSupportOrders = hadChildSupportOrder;

                        // Always reset the caretaker delete reason.
                        ncc.DeleteReasonId = null;
                    }
                }

                if (hadAnyChildSupportOrders)
                {
                    ncps.ChildSupportPayment        = model.ChildSupportPayment.ToDecimal();
                    ncps.HasOwedChildSupport        = model.HasOwedChildSupport;
                    ncps.HasInterestInChildServices = model.HasInterestInChildServices;
                }
                else
                {
                    ncps.ChildSupportPayment        = null;
                    ncps.HasOwedChildSupport        = null;
                    ncps.HasInterestInChildServices = null;
                }

                ncps.ChildSupportContactId = model.ChildSupportContactId;
                ncps.Notes                 = model.Notes;

                var currentIA = Repo.GetMostRecentAssessment(part);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;

                // Action Needed
                //if (model.ActionNeeded != null)
                //{
                //    UpdateActionBridge(model.ActionNeeded, ncpas, ncpas.AllNonCustodialParentsActionBridges.Cast<IActionBridge>().ToList(),
                //        id =>
                //        {
                //            var ab = Repo.NewNonCustodialParentsActionBridge(ncpas, user);
                //            ab.ActionNeededId = id;
                //        });
                //}

                // Do a concurrency check.
                if (!Repo.IsRowVersionStillCurrent(ncps, userRowVersion))
                {
                    return false;
                }

                if (!Repo.IsRowVersionStillCurrent(ncpas, userAssRowVersion))
                {
                    return false;
                }

                //Debug.WriteLine(ncps.AllNonCustodialCaretakers.Count);
                //foreach (var x in ncps.AllNonCustodialCaretakers)
                //{
                //    foreach (var xx in x.NonCustodialChilds)
                //    {
                //        Debug.WriteLine(xx.FirstName);
                //        Debug.WriteLine(xx.DeleteReasonId);
                //    }

                //}

                // If the first save completes, it actually has already saved the ChildYouthSupportsAssessmentSection
                // object as well since they are on the save repository context.  But if the ChildYouthSection didn't
                // need saving, we still need to SaveIfChangd on the ChildYouthSupportsAssessmentSection.
                if (!Repo.SaveIfChanged(ncps, user))
                    Repo.SaveIfChanged(ncpas, user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        /// <summary>
        /// Processes the posted Non Custodial Child data and updates the repository entities with the posted
        /// data.  It also returns and indicator if any of the posted children had a child support order.
        /// </summary>
        /// <param name="caretaker"></param>
        /// <param name="contractChildren"></param>
        /// <param name="contractDeletedChildren"></param>
        /// <param name="user"></param>
        /// <returns>True or False indicating if any of the posted children had a child support order.</returns>
        private bool ProcessCaretakerChildren(INonCustodialCaretaker caretaker, List<NonCustodialChildContract> contractChildren, List<NonCustodialChildContract> contractDeletedChildren, string user)
        {
            Debug.Assert(caretaker != null);

            bool hadAnyChildSupportOrders = false;

            // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
            var childrenInContract = contractChildren.WithoutEmpties();

            // If the caretaker is new then all the children will be new as well.
            if (caretaker.Id == 0)
            {
                // Since the caretaker is new, all the
                foreach (var childInContract in childrenInContract)
                {
                    INonCustodialChild ncc = Repo.NewNonCustodialChild(caretaker, user);

                    ncc.FirstName                = childInContract.FirstName;
                    ncc.LastName                 = childInContract.LastName;
                    ncc.DateOfBirth              = childInContract.DateOfBirth.ToDateTimeMonthDayYear();
                    ncc.HasChildSupportOrder     = childInContract.HasChildSupportOrder;
                    ncc.ChildSupportOrderDetails = childInContract.ChildSupportOrderDetails;

                    if (childInContract.HasChildSupportOrder == true)
                    {
                        childInContract.HasNameOnChildBirthRecord = null;
                    }

                    ncc.HasNameOnChildBirthRecord            = childInContract.HasNameOnChildBirthRecord;
                    ncc.ContactIntervalId                    = childInContract.ContactIntervalId;
                    ncc.ContactIntervalDetails               = childInContract.ContactIntervalDetails;
                    ncc.HasOtherAdultsYesNoUnknownLookupId   = childInContract.HasOtherAdultsPolarLookupId;
                    ncc.OtherAdultsDetails                   = childInContract.OtherAdultsDetails;
                    ncc.IsRelationshipChangeRequested        = childInContract.IsRelationshipChangeRequested;
                    ncc.RelationshipChangeRequestedDetails   = childInContract.RelationshipChangeRequestedDetails;
                    ncc.IsNeedOfServicesYesNoUnknownLookupId = childInContract.IsNeedOfServicesPolarLookupId;
                    ncc.NeedOfServicesDetails                = childInContract.NeedOfServicesDetails;
                    ncc.DeleteReasonId                       = null;

                    // We only want to flip the hadAnyChildSupportOrders to true if it hasn't already been flipped.
                    if (!hadAnyChildSupportOrders)
                    {
                        hadAnyChildSupportOrders = childInContract.HasChildSupportOrder.HasValue && childInContract.HasChildSupportOrder.Value;
                    }
                }
            }
            else
            {
                // Since the Caretaker already exists, there may be some existing Children we need to
                // update or some ones that have been dropped that need to be deleted.

                // Grab all the Children records for this Caretaker as we will need to delete the existing
                // ones that are not in the posted data.

                var caretakerChildren = caretaker.NonCustodialChilds.ToList();

                // If we were going to do any restore of soft-deleted items we would do it here.

                // Get the Id's of the Child records that already exist.
                var childIds = (from x in childrenInContract where x.Id != 0 select x.Id).ToList();

                // Update the existing children collection by marking those that have been deleted with a reason. 
                if (contractDeletedChildren != null)
                {
                    foreach (var item in contractDeletedChildren)
                    {
                        var deletedItem = caretaker.NonCustodialChilds.FirstOrDefault(x => x.Id == item.Id);

                        if (deletedItem != null)
                        {
                            deletedItem.DeleteReasonId = item.DeleteReasonId;
                        }
                    }
                }

                foreach (var contractChild in childrenInContract)
                {
                    INonCustodialChild child;

                    if (contractChild.IsNew())
                    {
                        child = Repo.NewNonCustodialChild(caretaker, user);
                    }
                    else
                    {
                        //child = (from x in caretakerChildren where x.Id == contractChild.Id select x).SingleOrDefault();
                        child = caretakerChildren.FirstOrDefault(i => i.Id == contractChild.Id);

                        if (child == null)
                        {
                            // must be a re-assignment from one ncp to another, find child in db
                            child = Repo.GetNonCustodialChild(i => i.Id == contractChild.Id); // or, Repo.GetNonCustodialChild(contractChild.Id); Linq-ified just looks cooler ;)

                            if (child == null)
                            {
                                Debug.Assert(child != null, "INonCustodialChild should not be null.");
                            }
                            else
                            {
                                child.NonCustodialCaretaker = caretaker;
                                //child.NonCustodialCaretakerId = caretaker.Id;
                            }
                        }
                    }

                    child.FirstName                = contractChild.FirstName;
                    child.LastName                 = contractChild.LastName;
                    child.DateOfBirth              = contractChild.DateOfBirth.ToDateTimeMonthDayYear();
                    child.HasChildSupportOrder     = contractChild.HasChildSupportOrder;
                    child.ChildSupportOrderDetails = contractChild.ChildSupportOrderDetails;

                    if (contractChild.HasChildSupportOrder == true)
                    {
                        contractChild.HasNameOnChildBirthRecord = null;
                    }

                    child.HasNameOnChildBirthRecord            = contractChild.HasNameOnChildBirthRecord;
                    child.ContactIntervalId                    = contractChild.ContactIntervalId;
                    child.ContactIntervalDetails               = contractChild.ContactIntervalDetails;
                    child.HasOtherAdultsYesNoUnknownLookupId   = contractChild.HasOtherAdultsPolarLookupId;
                    child.OtherAdultsDetails                   = contractChild.OtherAdultsDetails;
                    child.IsRelationshipChangeRequested        = contractChild.IsRelationshipChangeRequested;
                    child.RelationshipChangeRequestedDetails   = contractChild.RelationshipChangeRequestedDetails;
                    child.IsNeedOfServicesYesNoUnknownLookupId = contractChild.IsNeedOfServicesPolarLookupId;
                    child.NeedOfServicesDetails                = contractChild.NeedOfServicesDetails;
                    // child.DeleteReasonId = null;

                    // We only want to flip the hadAnyChildSupportOrders to true if it hasn't already been flipped.
                    if (!hadAnyChildSupportOrders)
                    {
                        hadAnyChildSupportOrders = contractChild.HasChildSupportOrder.HasValue && contractChild.HasChildSupportOrder.Value;
                    }
                }
            }

            return hadAnyChildSupportOrders;
        }

        public static NonCustodialParentAssessmentContract GetContract(IInformalAssessment ia, IParticipant part, IRepository repo)
        {
            var contract = new NonCustodialParentAssessmentContract();
            contract.NonCustodialCaretakers = new List<NonCustodialCaretakerContract>();

            if (ia != null && part != null)
            {
                var ncp = part.NonCustodialParentsSection;

                // Apply all the contract properties from the Participant based tables:
                if (ncp != null)
                {
                    contract.HasChildren = ncp.HasChildren;

                    if (ncp.NonCustodialCaretakers != null)
                    {
                        foreach (var caretaker in ncp.NonCustodialCaretakers.Where(x => x.DeleteReasonId == null))
                        {
                            var careContract = new NonCustodialCaretakerContract
                                               {
                                                   Id                                      = caretaker.Id,
                                                   FirstName                               = caretaker.FirstName,
                                                   IsFirstNameUnknown                      = caretaker.IsFirstNameUnknown,
                                                   LastName                                = caretaker.LastName,
                                                   IsLastNameUnknown                       = caretaker.IsLastNameUnknown,
                                                   NonCustodialParentRelationshipId        = caretaker.NonCustodialParentRelationshipId,
                                                   NonCustodialParentRelationshipName      = caretaker.NonCustodialParentRelationship?.Name,
                                                   RelationshipDetails                     = caretaker.RelationshipDetails,
                                                   ContactIntervalId                       = caretaker.ContactIntervalId,
                                                   ContactIntervalName                     = caretaker.ContactInterval?.Name,
                                                   ContactIntervalDetails                  = caretaker.ContactIntervalDetails,
                                                   IsRelationshipChangeRequested           = caretaker.IsRelationshipChangeRequested,
                                                   RelationshipChangeRequestedDetails      = caretaker.RelationshipChangeRequestedDetails,
                                                   IsInterestedInRelationshipReferral      = caretaker.IsInterestedInRelationshipReferral,
                                                   InterestedInRelationshipReferralDetails = caretaker.InterestedInRelationshipReferralDetails,
                                                   RowVersion                              = caretaker.RowVersion
                                               };

                            if (caretaker.NonCustodialChilds != null)
                            {
                                careContract.NonCustodialChilds = new List<NonCustodialChildContract>();

                                foreach (var child in caretaker.NonCustodialChilds.Where(x => x.DeleteReasonId == null))
                                {
                                    var childContract = new NonCustodialChildContract
                                                        {
                                                            Id                                 = child.Id,
                                                            FirstName                          = child.FirstName,
                                                            LastName                           = child.LastName,
                                                            DateOfBirth                        = child.DateOfBirth.ToStringMonthDayYear(),
                                                            HasChildSupportOrder               = child.HasChildSupportOrder,
                                                            ChildSupportOrderDetails           = child.ChildSupportOrderDetails,
                                                            HasNameOnChildBirthRecord          = child.HasNameOnChildBirthRecord,
                                                            ContactIntervalId                  = child.ContactIntervalId,
                                                            ContactIntervalName                = child.ContactInterval?.Name,
                                                            ContactIntervalDetails             = child.ContactIntervalDetails,
                                                            HasOtherAdultsPolarLookupId        = child.HasOtherAdultsYesNoUnknownLookupId,
                                                            HasOtherAdultsPolarLookupName      = child.HasOtherAdultsYesNoUnknownLookup?.Name,
                                                            OtherAdultsDetails                 = child.OtherAdultsDetails,
                                                            IsRelationshipChangeRequested      = child.IsRelationshipChangeRequested,
                                                            RelationshipChangeRequestedDetails = child.RelationshipChangeRequestedDetails,
                                                            IsNeedOfServicesPolarLookupId      = child.IsNeedOfServicesYesNoUnknownLookupId,
                                                            IsNeedOfServicesPolarLookupName    = child.IsNeedOfServicesYesNoUnknownLookup?.Name,
                                                            NeedOfServicesDetails              = child.NeedOfServicesDetails,
                                                            RowVersion                         = child.RowVersion
                                                        };

                                    careContract.NonCustodialChilds.Add(childContract);
                                }
                            }

                            contract.NonCustodialCaretakers.Add(careContract);
                        }
                    }

                    contract.ChildSupportPayment                 = ncp.ChildSupportPayment?.ToString();
                    contract.HasOwedChildSupport                 = ncp.HasOwedChildSupport;
                    contract.HasInterestInChildServices          = ncp.HasInterestInChildServices;
                    contract.IsInterestedInReferralServices      = ncp.IsInterestedInReferralServices;
                    contract.InterestedInReferralServicesDetails = ncp.InterestedInReferralServicesDetails;
                    contract.ChildSupportContactId               = ncp.ChildSupportContactId;
                    contract.Notes                               = ncp.Notes;

                    // Standard modified by/rowversion stuff
                    contract.RowVersion   = ncp.RowVersion;
                    contract.ModifiedBy   = ncp.ModifiedBy;
                    contract.ModifiedDate = ncp.ModifiedDate;
                }

                if (ia.NonCustodialParentsAssessmentSection != null)
                {
                    contract.IsSubmittedViaDriverFlow = true;

                    // Make a call to the helper method in the base class to set the assessment row version and update
                    // modified stuff if needed.
                    UpdateRowVersionAndModifiedIfAssessmentMoreRecent(contract, ncp, ia.NonCustodialParentsAssessmentSection);
                }

                contract.ActionNeeded = ActionNeededViewModel.GetActionNeededContract(part, repo, Constants.ActionNeededPage.NonCustodialParents);
            }

            return contract;
        }
    }
}
