using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Api.Library.Utils;


namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class NonCustodialParentsReferralSectionViewModel : BaseInformalAssessmentViewModel
    {
        public NonCustodialParentsReferralSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser) { }

        public NonCustodialParentReferralAssessmentContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }
        
        public bool PostData(NonCustodialParentReferralAssessmentContract contract, string user)
        {
            var informalAssessment = InformalAssessment;
            var participant        = Participant;

            if (participant == null)
            {
                throw new InvalidOperationException("PIN not valid.");
            }


            if (contract == null)
            {
                throw new InvalidOperationException("Non Custodial Parent data is missing.");
            }

            if (informalAssessment == null)
            {
                throw new InvalidOperationException("InformalAssessment record is missing.");
            }

            //var modifiedDate = DateTime.Now;
            var updateTime = DateTime.Now;

            INonCustodialParentsReferralAssessmentSection ncpReferralAssementSection = null;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (participant.InProgressInformalAssessment != null)
            {
                ncpReferralAssementSection = participant.InProgressInformalAssessment.NonCustodialParentsReferralAssessmentSection ?? Repo.NewNonCustodialParentsReferralAssessmentSection(participant.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(ncpReferralAssementSection);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                ncpReferralAssementSection.ReviewCompleted = true;
                ncpReferralAssementSection.ModifiedBy      = AuthUser.Username;
                ncpReferralAssementSection.ModifiedDate    = updateTime;
            }

            var ncpReferralSection = participant.NonCustodialParentsReferralSection ?? Repo.NewNonCustodialParentsReferralSection(participant.Id, user);
            ncpReferralSection.ModifiedBy   = AuthUser.Username;
            ncpReferralSection.ModifiedDate = updateTime;
            Repo.StartChangeTracking(ncpReferralSection);

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            ncpReferralSection.HasChildrenId = contract.HasChildrenId;
            ncpReferralSection.Notes         = contract.Notes;
            // If for any weird reason this was deleted we are going to un-delete it.
            ncpReferralSection.IsDeleted = false;

            // Grab all the parent records from the database which includes the soft deleted item.
            var       allParents = ncpReferralSection.AllNonCustodialReferralParents.ToList();
            List<int> parentIds;

            if (contract.HasChildrenId.HasValue && contract.HasChildrenId.Value == YesNoSkipLookup.YesId)
            {
                // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.
                contract.Parents = contract.Parents.WithoutEmpties();

                // NOTE: We don't do any restore process for existing/deleted items.  If we did, it
                // would be done here.

                // Get the Id's of the NonCustodialCaretakers that are not new.
                //parentIds = (from x in contract.Parents where x.Id != 0 select x.Id).ToList();
                parentIds = contract.Parents.Where(i => i.Id != 0).Select(i => i.Id).ToList();
            }
            else
            {
                // The simplest thing to do if the posted contract data indicates there should
                // be no repeater records is to just clear out any incoming records.
                // That will allow the normal logic below to just mark any existing records as
                // deleted, even if they were posted in the data contract.
                contract.Parents = null;

                // Set the repeater IDs to an empty list since there aren't any.  The effect of
                // this empty list of active IDs is that all the existing Caretaker objects will
                // be marked as deleted.
                parentIds = new List<int>();
            }

            // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
            // to mark the unused items as deleted.
            //allParents.MarkUnusedItemsAsDeleted(parentIds);
            // Set Delete Reason Ids for delete Data.
            if (contract.Parents != null)
            {
                foreach (var parentInContract in contract.Parents)
                {
                    var parentinSection = ncpReferralSection.NonCustodialReferralParents.FirstOrDefault(i => i.Id == parentInContract.Id);
                    if (parentinSection != null)
                    {
                        parentinSection.DeleteReasonId = parentInContract.DeleteReasonId;
                    }
                }
            }

            // Deleted items come in via their own collection
            if (contract.DeletedParents != null)
            {
                foreach (var deletedParentInContract in contract.DeletedParents)
                {
                    var ncrp = allParents.FirstOrDefault(i => i.Id == deletedParentInContract.Id);

                    if (ncrp != null)
                    {
                        ncrp.DeleteReasonId = deletedParentInContract.DeleteReasonId;
                    }

                    if (deletedParentInContract.DeletedChildren != null)
                    {
                        foreach (var deletedChild in deletedParentInContract.DeletedChildren)
                        {
                            var ncrc = Repo.GetNonCustodialReferralChildById(deletedChild.Id);

                            if (ncrc != null)
                            {
                                ncrc.DeleteReasonId = deletedChild.DeleteReasonId;
                            }
                        }
                    }
                }
            }

            // Now update the database items with the posted model data.
            if (contract.Parents != null)
            {
                foreach (var parentInContract in contract.Parents)
                {
                    INonCustodialReferralParent ncrp;

                    if (parentInContract.IsNew())
                    {
                        ncrp              = Repo.NewNonCustodialReferralParent(ncpReferralSection, user);
                        ncrp.ModifiedDate = updateTime;
                        ncrp.ModifiedBy   = user;
                    }
                    else
                    {
                        ncrp = (from x in allParents where x.Id == parentInContract.Id select x).SingleOrDefault();
                    }

                    //Debug.Assert(ncrp != null, "INonCustodialReferralParent should not be null.");

                    ncrp.FirstName                      = parentInContract.FirstName;
                    ncrp.LastName                       = parentInContract.LastName;
                    ncrp.IsAvailableOrWorking           = parentInContract.IsAvailableOrWorking;
                    ncrp.AvailableOrWorkingDetails      = parentInContract.AvailableOrWorkingDetails;
                    ncrp.IsInterestedInWorkProgram      = parentInContract.IsInterestedInWorkProgram;
                    ncrp.InterestedInWorkProgramDetails = parentInContract.InterestedInWorkProgramDetails;
                    ncrp.IsContactKnownWithParent       = parentInContract.IsContactKnownWithParent;
                    ncrp.ContactId                      = parentInContract.IsContactKnownWithParent == true ? parentInContract.ContactId : null;

                    // Process the repeater full of children.
                    //ProcessChildren(ncrp, parentInContract.Children, user);
                    ProcessChildren(ncrp, parentInContract, user);

                    // Always reset the soft delete.
                    ncrp.DeleteReasonId = null;
                }
            }

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(ncpReferralSection, userRowVersion))
            {
                return (false);
            }


            if (!Repo.IsRowVersionStillCurrent(ncpReferralAssementSection, userAssessRowVersion))
            {
                return (false);
            }

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.

            if (participant.InProgressInformalAssessment != null)
            {
                var currentIA = Repo.GetMostRecentAssessment(participant);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;
            }

            if (!Repo.SaveIfChanged(ncpReferralSection, user))
            {
                Repo.SaveIfChanged(ncpReferralAssementSection, user);
            }

            return (true);
        }

        private void ProcessChildren(INonCustodialReferralParent parent, NonCustodialReferralParentContract parentInContract, string user)
        {
            //Debug.Assert(parent != null);

            // First, cleanse the incoming repeater data.  This means clearing out empty repeater items.

            var childrenInContract = parentInContract.Children.WithoutEmpties();

            // If the caretaker is new then all the children will be new as well.
            if (parent.Id == 0)
            {
                // Since the caretaker is new, all the
                foreach (var childInContract in childrenInContract)
                {
                    var ncrc = Repo.NewNonCustodialReferralChild(parent, user);

                    ncrc.FirstName                 = childInContract.FirstName;
                    ncrc.LastName                  = childInContract.LastName;
                    ncrc.ReferralContactIntervalId = childInContract.ContactIntervalId;
                    ncrc.ContactIntervalDetails    = childInContract.ContactIntervalDetails;

                    if (childInContract.ContactIntervalId == ReferralContactInterval.OtherParentIsDeceasedId)
                    {
                        ncrc.HasChildSupportOrder     = null;
                        ncrc.ChildSupportOrderDetails = null;
                    }
                    else
                    {
                        ncrc.HasChildSupportOrder     = childInContract.HasChildSupportOrder;
                        ncrc.ChildSupportOrderDetails = childInContract.ChildSupportOrderDetails;
                    }

                    ncrc.DeleteReasonId = null;
                }
            }
            else
            {
                // Since the Parent already exists, there may be some existing Children we need to
                // update or some ones that have been dropped that need to be deleted.

                // Grab all the Children records for this Parent as we will need to soft-delete the existing
                // ones that are not in the posted data.
                var existingChildren = parent.NonCustodialReferralChilds.ToList();

                // If we were going to do any restore of soft-deleted items we would do it *here*.

                // Get the Id's of the Child records that already exist.
                // Update the existing children collection by marking those that have not been posted back  as deleted.
                // Set Delete Reason Ids for delete Data.
                if (parentInContract.DeletedChildren != null)
                {
                    foreach (var deletedChildInContract in parentInContract.DeletedChildren)
                    {
                        var deletedChild = existingChildren.FirstOrDefault(x => x.Id == deletedChildInContract.Id);
                        if (deletedChild != null)
                        {
                            deletedChild.DeleteReasonId = deletedChildInContract.DeleteReasonId;
                        }
                    }
                }

                foreach (var contractChild in childrenInContract)
                {
                    INonCustodialReferralChild child;

                    if (contractChild.IsNew())
                    {
                        child = Repo.NewNonCustodialReferralChild(parent, user);
                    }
                    else
                    {
                        child = existingChildren.FirstOrDefault(i => i.Id == contractChild.Id);

                        if (child == null)
                        {
                            // must be a re-assignment from one ncp to another, find child in db
                            child = Repo.GetNonCustodialReferralChild(i => i.Id == contractChild.Id); // or, Repo.GetNonCustodialReferralChild(contractChild.Id); but the lambda just looks cooler ;)

                            if (child == null)
                            {
                                Debug.Assert(child != null, "INonCustodialReferralChild should not be null.");
                            }
                            else
                            {
                                child.NonCustodialReferralParent = parent;
                                //child.NonCustodialReferralParentId = parent.Id;
                            }
                        }
                    }

                    child.FirstName                 = contractChild.FirstName;
                    child.LastName                  = contractChild.LastName;
                    child.ReferralContactIntervalId = contractChild.ContactIntervalId;
                    child.ContactIntervalDetails    = contractChild.ContactIntervalDetails;

                    if (contractChild.ContactIntervalId == ReferralContactInterval.OtherParentIsDeceasedId)
                    {
                        child.HasChildSupportOrder     = null;
                        child.ChildSupportOrderDetails = null;
                    }
                    else
                    {
                        child.HasChildSupportOrder     = contractChild.HasChildSupportOrder;
                        child.ChildSupportOrderDetails = contractChild.ChildSupportOrderDetails;
                    }

                    child.DeleteReasonId = null;
                }
            }
        }

        public static NonCustodialParentReferralAssessmentContract GetContract(IInformalAssessment ia, IParticipant part, IRepository repo)
        {
            var contract = new NonCustodialParentReferralAssessmentContract
                           {
                               Parents     = new List<NonCustodialReferralParentContract>(),
                               CwwChildren = CwwHelper.GetCwwChildren(repo, part)
                           };

            // CWW referential data

            if (ia == null) return contract;

            if (part?.NonCustodialParentsReferralSection == null) return contract;

            var ncprs = part.NonCustodialParentsReferralSection;

            contract.HasChildrenId   = ncprs.HasChildrenId;
            contract.HasChildrenName = ncprs.YesNoSkipLookup?.Name;

            if (ncprs.NonCustodialReferralParents != null)
            {
                foreach (var parent in ncprs.NonCustodialReferralParents)
                {
                    var parentContract = NCRHelper.GetNCRParent(parent);

                    if (parent.NonCustodialReferralChilds != null)
                    {
                        parentContract.Children = NCRHelper.GetNCRChildren(parent);
                    }

                    contract.Parents.Add(parentContract);
                }

                contract.Notes = ncprs.Notes;

                // Standard modified by/rowversion stuff
                contract.RowVersion   = ncprs.RowVersion;
                contract.ModifiedBy   = ncprs.ModifiedBy;
                contract.ModifiedDate = ncprs.ModifiedDate;
            }

            // We look at the assessment section now which at this point just
            // indicates it was submitted via the driver flow.
            if (ia.NonCustodialParentsReferralAssessmentSection != null)
            {
                contract.AssessmentRowVersion     = ia.NonCustodialParentsReferralAssessmentSection.RowVersion;
                contract.IsSubmittedViaDriverFlow = true;
            }

            return contract;
        }
    }
}
