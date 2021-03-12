using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Constants = Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class LegalIssueSectionViewModel : BaseInformalAssessmentViewModel
    {
        public LegalIssueSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public LegalIssuesSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        // repeater deleted ids for Pending Charges 
        private static bool DeleteFromList(ICollection<IPendingCharge> list, List<int> modelIds, List<int> dbIds)
        {
            var deletedList = dbIds.Except(modelIds).ToArray();

            foreach (int n in deletedList)
            {
                var c = list.First(x => x.Id == n);
                c.IsDeleted = true;
            }

            return true;
        }

        // repeater deleted ids for Court dates 
        private static bool DeleteFromList(ICollection<ICourtDate> list, List<int> modelIds, List<int> dbIds)
        {
            var deletedList = dbIds.Except(modelIds).ToArray();

            foreach (int n in deletedList)
            {
                var c = list.First(x => x.Id == n);
                c.IsDeleted = true;
            }

            return true;
        }

        public bool PostData(LegalIssuesSectionContract contract, string user)
        {
            var p    = Participant;
            var ia   = InformalAssessment;
            var part = Participant;

            if (p == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("TransportationSectionContract is missing.");

            if (ia == null)
                throw new InvalidOperationException("InformalAssessment record is missing.");

            ILegalIssuesSection           lis  = null;
            ILegalIssuesAssessmentSection lias = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                lias = p.InProgressInformalAssessment.LegalIssuesAssessmentSection ?? Repo.NewLegalIssuesAssessmentSection(p.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(lias);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                lias.ReviewCompleted = true;
                lias.ModifiedBy   = AuthUser.Username;
                lias.ModifiedDate = updateTime;
            }

            lis = p.LegalIssuesSection ?? Repo.NewLegalIssuesSection(p, user);
            lis.ModifiedBy   = AuthUser.Username;
            lis.ModifiedDate = updateTime;

            Repo.StartChangeTracking(lis);

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            lis.IsConvictedOfCrime = contract.IsConvictedOfCrime;
            var allConvictions = lis.AllConvictions.ToList();

            // Check if they answered yes for Crime Conviction.
            if (contract.IsConvictedOfCrime.HasValue && contract.IsConvictedOfCrime.Value)
            {
                lis.IsUnderCommunitySupervision = contract.IsUnderCommunitySupervision;
                // Step 1/4 in handling repeaters: Remove Blanks from contract's collection.
                contract.Convictions = contract.Convictions.WithoutEmpties();

                // Step 2/4 in handling repeaters: Mapping.
                contract.Convictions.UpdateNewItemsIfSimilarToExisting(allConvictions, ConvictionContract.AdoptIfSimilarToModel);

                // Step 3/4  
                foreach (var crime in contract.Convictions)
                {
                    if (crime.Id            == 0    && string.IsNullOrEmpty(crime.Date) && string.IsNullOrEmpty(crime.Details) &&
                        crime.IsDateUnknown == null && crime.Type == null)
                    {
                        continue;
                    }

                    IConviction c = null;
                    c = lis.AllConvictions.SingleOrDefault(x => x.Id == crime.Id && x.Id != 0);

                    if (c == null || c.Id == 0)
                    {
                        c = Repo.NewConviction(lis, user);
                    }

                    c.ConvictionType = Repo.ConvictionTypeById(crime.Type);
                    c.IsUnknown      = crime.IsDateUnknown;

                    if (c.IsUnknown.HasValue && c.IsUnknown.Value)
                    {
                        c.DateConvicted = null;
                    }
                    else
                    {
                        c.DateConvicted = crime.Date?.ToDateTimeMonthYear();
                    }

                    c.IsUnknown      = crime.IsDateUnknown;
                    c.Details        = crime.Details;
                    c.ModifiedBy     = user;
                    c.ModifiedDate   = updateTime;
                    c.DeleteReasonId = null;
                    lis.Convictions.Add(c);
                }
            }
            else
            {
                lis.IsUnderCommunitySupervision = null;

                foreach (var i in lis.Convictions)
                {
                    i.DeleteReasonId = DeleteReasonLookup.EnteredInError;
                }
            }

            if (contract.DeletedConvictions != null)
            {
                foreach (var dContract in contract.DeletedConvictions)
                {
                    if (dContract.Id != 0)
                    {
                        var dModel = lis.AllConvictions.FirstOrDefault(x => x.Id == dContract.Id);

                        if (dModel != null)
                            dModel.DeleteReasonId = dContract.DeleteReasonId;
                    }
                }
            }

            // Check if they answered yes for undergoing any Community Supervision. 

            if (lis.IsUnderCommunitySupervision.HasValue && lis.IsUnderCommunitySupervision.Value)
            {
                lis.CommunitySupervisonDetails   = contract.CommunitySupervisonDetails;
                lis.CommunitySupervisonContactId = contract.SupervisionContactId;

                if (contract.SupervisionContactId != 0)
                {
                    lis.CommunitySupervisonContactId = contract.SupervisionContactId;
                }
                else
                {
                    lis.CommunitySupervisonContactId = null;
                }
            }
            else
            {
                lis.CommunitySupervisonDetails = null;
            }

            lis.HasPendingCharges = contract.IsPending;

            if (lis.HasPendingCharges.HasValue && lis.HasPendingCharges.Value)
            {
                foreach (var pe in contract.Pendings)
                {
                    //var dob = Participant.DateOfBirth;
                    //if (pendings.Date.ToDateTimeMonthYear() >= dob && pendings.Date.ToIntYear() >= dob.ToIntYear()+150)
                    //{
                    //    throw new InvalidOperationException("Pending charges date cannot be before date of birth.");
                    //}
                    if (pe.Id != 0 && pe.Type == null && pe.IsDateUnknown == null && pe.Type == null && string.IsNullOrEmpty(pe.Date) && string.IsNullOrEmpty(pe.Details))
                    {
                        pe.Id = 0;
                    }
                }

                // Model list.
                var pendingchargesByIdModel = (from x in contract.Pendings select x.Id).ToList();
                // Database list.
                var pendingchargesByIdDb = (from x in lis.PendingCharges select x.Id).ToList();

                // Here we soft delete CrimeConventions that have been deleted by
                // the user. First we create the list of CrimeConventionsIDs that 
                // are missing from the upsert model. 
                DeleteFromList(lis.PendingCharges, pendingchargesByIdModel, pendingchargesByIdDb);

                #region Restoration of CrimeConventions

                var pendingchargesModel = contract.Pendings;

                // List of deleted CrimeConvention lists from Database.   
                var deletedPendingchargeDb = from x in lis.PendingCharges where x.IsDeleted == true select x;

                var PendingChargesRestoreList = new List<PendingContract>();

                foreach (var d in deletedPendingchargeDb)
                {
                    foreach (var m in pendingchargesModel)
                    {
                        if (
                                d.ChargeDate       == m.Date?.ToDateTimeMonthYear() &&
                                d.IsUnknown        == m.IsDateUnknown               &&
                                d.Details          == m.Details                     &&
                                d.ConvictionTypeID == m.Type
                            )
                        {
                            d.IsDeleted = false;
                            PendingChargesRestoreList.Add(m);
                        }
                    }
                }

                foreach (var r in PendingChargesRestoreList)
                {
                    contract.Pendings.Remove(r);
                }

                #endregion

                foreach (var pendingcharge in contract.Pendings)
                {
                    if (pendingcharge.Id == 0 && pendingcharge.Type == null && pendingcharge.IsDateUnknown == null && pendingcharge.Type == null && string.IsNullOrEmpty(pendingcharge.Date) && string.IsNullOrEmpty(pendingcharge.Details))
                    {
                        continue;
                    }

                    IPendingCharge pc = null;
                    pc = lis.PendingCharges.SingleOrDefault(x => x.Id == pendingcharge.Id && x.Id != 0);

                    if (pc == null || pc.Id == 0)
                    {
                        pc = Repo.NewPendingCharge(lis, user);
                    }

                    pc.ConvictionType = Repo.ConvictionTypeById(pendingcharge.Type);
                    pc.IsUnknown      = pendingcharge.IsDateUnknown;

                    if (pc.IsUnknown.HasValue && pc.IsUnknown.Value)
                    {
                        pc.ChargeDate = null;
                    }
                    else
                    {
                        pc.ChargeDate = pendingcharge.Date?.ToDateTimeMonthYear();
                    }

                    pc.Details      = pendingcharge.Details;
                    pc.ModifiedBy   = user;
                    pc.ModifiedDate = updateTime;
                    lis.PendingCharges.Add(pc);
                }
            }
            else
            {
                foreach (var l in lis.PendingCharges)
                {
                    l.IsDeleted = true;
                }
            }

            // Check if there is curerently any restraining order.
            lis.HasRestrainingOrders = contract.HasRestrainingOrders;

            if (lis.HasRestrainingOrders.HasValue && lis.HasRestrainingOrders.Value)
            {
                lis.RestrainingOrderNotes = contract.RestrainingOrderNotes;
            }
            else
            {
                lis.RestrainingOrderNotes = null;
            }

            // Check if there is any restraining order to prevent that person from contacting.
            lis.HasRestrainingOrderToPrevent = contract.HasRestrainingOrderToPrevent;

            if (lis.HasRestrainingOrderToPrevent.HasValue && lis.HasRestrainingOrderToPrevent.Value)
            {
                lis.RestrainingOrderToPreventNotes = contract.RestrainingOrderToPreventNotes;
            }
            else
            {
                lis.RestrainingOrderToPreventNotes = null;
            }

            // Check if family members having legal issues.
            lis.HasFamilyLegalIssues = contract.HasFamilyLegalIssues;

            if (lis.HasFamilyLegalIssues.HasValue && lis.HasFamilyLegalIssues.Value)
            {
                lis.FamilyLegalIssueNotes = contract.FamilyLegalIssueNotes;
            }
            else
            {
                lis.Notes = null;
            }

            // Are you currently ordered to pay child support?
            //model.OrderedToPayChildSupport = contract.HasChildSupport;
            //if (model.OrderedToPayChildSupport.HasValue && model.OrderedToPayChildSupport.Value)
            //{
            //    // Monthly Amount.
            //    if (contract.IsAmountUnknown == true)
            //    {
            //        model.IsUnknown = contract.IsAmountUnknown;
            //        model.MonthlyAmount = null;
            //    }
            //    else
            //    {
            //        model.IsUnknown = false;
            //        if (contract.ChildSupportAmount.Length == 0)
            //        {
            //            model.MonthlyAmount = null;
            //        }
            //        else
            //        {
            //            model.MonthlyAmount = System.Convert.ToDecimal(contract.ChildSupportAmount); ;
            //        }

            //    }
            //    model.OweAnyChildSupportBack = contract.HasBackChildSupport;
            //    model.ChildSupportDetails = contract.ChildSupportDetails;
            //}

            lis.HasCourtDates = contract.HasUpcomingCourtDates;

            if (lis.HasCourtDates.HasValue && lis.HasCourtDates.Value)
            {
                foreach (var courtdate in contract.CourtDates)
                {
                    //var dob = Participant.DateOfBirth;
                    //if (courtdate.Date.ToDateTimeMonthYear() <= dob)
                    //{
                    //    throw new InvalidOperationException("Court date cannot be before date of birth.");
                    //}
                    if (courtdate.Id != 0                       && courtdate.IsDateUnknown == null && string.IsNullOrEmpty(courtdate.Date) &&
                        string.IsNullOrEmpty(courtdate.Details) && string.IsNullOrEmpty(courtdate.Location))
                    {
                        courtdate.Id = 0;
                    }
                }

                // Check for upcoming court dates.
                lis.HasCourtDates = contract.HasUpcomingCourtDates;
                // Model list.
                var courtDatesByIdModel = (from x in contract.CourtDates select x.Id).ToList();
                // Databaselist.
                var courtDatesByIdDb = (from x in lis.CourtDates select x.Id).ToList();

                // Here we soft delete CourtDates that have been deleted by
                // the user. First we create the list of CourtDatesIDs that 
                // are missing from the upsert model. 
                DeleteFromList(lis.CourtDates, courtDatesByIdModel, courtDatesByIdDb);

                #region Restoration of CourtDates

                var courtDatesModel = contract.CourtDates;

                // List of deleted CrimeConvention lists from Database.   
                var deletedCourtDateDb = from x in lis.CourtDates where x.IsDeleted == true select x;

                var courtDatesRestoreList = new List<CourtContract>();

                foreach (var d in deletedCourtDateDb)
                {
                    foreach (var m in courtDatesModel)
                    {
                        if (d.Location  == m.Location                   &&
                            d.Date      == m.Date.ToDateTimeMonthYear() &&
                            d.IsUnknown == m.IsDateUnknown              &&
                            d.Details   == m.Details
                            )
                        {
                            d.IsDeleted = false;
                            courtDatesRestoreList.Add(m);
                        }
                    }
                }

                foreach (var r in courtDatesRestoreList)
                {
                    contract.CourtDates.Remove(r);
                }

                #endregion

                foreach (var courtDate in contract.CourtDates)
                {
                    if (courtDate.Id == 0                       && courtDate.IsDateUnknown == null && string.IsNullOrEmpty(courtDate.Date) &&
                        string.IsNullOrEmpty(courtDate.Details) && string.IsNullOrEmpty(courtDate.Location))
                    {
                        continue;
                    }

                    ICourtDate c = null;
                    c = lis.CourtDates.SingleOrDefault(x => x.Id == courtDate.Id && x.Id != 0);

                    if (c == null || c.Id == 0)
                    {
                        c = Repo.NewCourtDate(lis, user);
                    }

                    c.Location  = courtDate.Location;
                    c.IsUnknown = courtDate.IsDateUnknown;

                    if (c.IsUnknown.HasValue && c.IsUnknown.Value)
                    {
                        c.Date = null;
                    }
                    else
                    {
                        c.Date = courtDate.Date?.ToDateTimeMonthDayYear();
                    }

                    c.Details      = courtDate.Details;
                    c.ModifiedBy   = user;
                    c.ModifiedDate = updateTime;
                }
            }
            else
            {
                foreach (var co in lis.CourtDates)
                {
                    co.IsDeleted = true;
                }
            }

            // Notes.
            lis.Notes = contract.Notes;
            var currentIA = Repo.GetMostRecentAssessment(p);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(lis, userRowVersion))
                return false;

            if (!Repo.IsRowVersionStillCurrent(lias, userAssessRowVersion))
                return false;

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.
            if (!Repo.SaveIfChanged(lis, user))
                Repo.SaveIfChanged(lias, user);

            return true;
        }

        public static LegalIssuesSectionContract GetContract(IInformalAssessment ia, IParticipant part, IRepository repo)
        {
            var contract = new LegalIssuesSectionContract();

            if (ia != null && part != null)
            {
                // Since the action needed is not dependent upon any section or assessment section data,
                // we'll add it right away.
                contract.ActionNeeded = ActionNeededViewModel.GetActionNeededContract(part, repo,
                                                                                      Constants.ActionNeededPage.LegalIssues);

                if (part.LegalIssuesSection != null)
                {
                    var model = part.LegalIssuesSection;

                    contract.RowVersion   = model.RowVersion;
                    contract.ModifiedBy   = model.ModifiedBy;
                    contract.ModifiedDate = model.ModifiedDate;

                    // Not including minor traffic violations, have you ever been convicted of a crime?
                    contract.IsConvictedOfCrime = model.IsConvictedOfCrime;

                    if (model.IsConvictedOfCrime == true)
                    {
                        //lid.IsConvictedOfCrime = model.IsConvictedOfCrime;
                        var ccl = new List<ConvictionContract>();

                        // Sorted unknowns first then by newest to oldest date.
                        foreach (var lic in model.Convictions.AsNotNull().OrderByDescending(x => x.IsUnknown).ThenByDescending(x => x.DateConvicted))
                        {
                            var cc = new ConvictionContract();
                            cc.Type     = lic.ConvictionTypeID;
                            cc.TypeName = lic.ConvictionType?.Name.SafeTrim();
                            // Radio Button for  date is Unknown.
                            cc.IsDateUnknown = lic.IsUnknown;
                            ;

                            if (lic.IsUnknown.HasValue && lic.IsUnknown.Value)
                            {
                                cc.Date = null;
                            }
                            else
                            {
                                cc.Date = lic.DateConvicted?.ToString("MM/yyyy");
                            }

                            cc.Details = lic.Details;
                            cc.Id      = lic.Id;
                            ccl.Add(cc);
                        }

                        contract.Convictions = ccl;
                    }
                    else
                    {
                        var c = new List<ConvictionContract>();
                        contract.Convictions = c;
                    }

                    // Are you currently under community supervision?
                    contract.IsUnderCommunitySupervision = model.IsUnderCommunitySupervision;

                    if (model.IsUnderCommunitySupervision == true)
                    {
                        contract.IsUnderCommunitySupervision = model.IsUnderCommunitySupervision;
                        contract.CommunitySupervisonDetails  = model.CommunitySupervisonDetails;
                        contract.SupervisionContactId        = model.CommunitySupervisonContactId;
                    }

                    // Do you have any pending charges?
                    contract.IsPending = model.HasPendingCharges;

                    if (model.HasPendingCharges == true)
                    {
                        var pcl = new List<PendingContract>();

                        foreach (var licp in model.PendingCharges)
                        {
                            var pc = new PendingContract();
                            pc.Type     = licp.ConvictionTypeID;
                            pc.TypeName = licp.ConvictionType?.Name.SafeTrim();
                            ;
                            // Radio Button for is date Unknown.                  
                            pc.IsDateUnknown = licp.IsUnknown;

                            if (licp.IsUnknown.HasValue && licp.IsUnknown.Value)
                            {
                                pc.Date = null;
                            }
                            else
                            {
                                pc.Date = licp.ChargeDate?.ToString("MM/yyyy");
                            }

                            pc.Details = licp.Details;
                            pc.Id      = licp.Id;
                            pcl.Add(pc);
                        }

                        contract.Pendings = pcl;
                    }
                    else
                    {
                        var p = new List<PendingContract>();
                        contract.Pendings = p;
                    }

                    // Are there currently any restraining orders against you?
                    contract.HasRestrainingOrders = model.HasRestrainingOrders;

                    if (model.HasRestrainingOrders == true)
                    {
                        contract.RestrainingOrderNotes = model.RestrainingOrderNotes;
                    }

                    // Do you currently have a restraining order against anyone to prevent that person from contacting you?
                    contract.HasRestrainingOrderToPrevent = model.HasRestrainingOrderToPrevent;

                    if (model.HasRestrainingOrderToPrevent == true)
                    {
                        contract.RestrainingOrderToPreventNotes = model.RestrainingOrderToPreventNotes;
                    }

                    // Do you have any immediate family members with legal issues?
                    contract.HasFamilyLegalIssues = model.HasFamilyLegalIssues;

                    if (model.HasFamilyLegalIssues == true)
                    {
                        contract.FamilyLegalIssueNotes = model.FamilyLegalIssueNotes;
                    }

                    contract.HasUpcomingCourtDates = model.HasCourtDates;

                    if (model.HasCourtDates == true)
                    {
                        var pcl = new List<CourtContract>();

                        foreach (var licd in model.CourtDates)
                        {
                            var cc = new CourtContract();
                            cc.Location = licd.Location;
                            // Radio Button for is date Unknown.                  
                            var isUnknown = licd.IsUnknown;
                            cc.IsDateUnknown = isUnknown;

                            if (licd.IsUnknown.HasValue && licd.IsUnknown.Value)
                            {
                                cc.Date = null;
                            }
                            else
                            {
                                cc.Date = licd.Date?.ToString("MM/dd/yyyy");
                            }

                            cc.Details = licd.Details;
                            cc.Id      = licd.Id;
                            pcl.Add(cc);
                        }

                        contract.CourtDates = pcl;
                    }
                    else
                    {
                        var cc = new List<CourtContract>();
                        contract.CourtDates = cc;
                    }

                    contract.Notes = model.Notes;
                }
                else
                {
                    var c = new List<ConvictionContract>();
                    contract.Convictions = c;

                    var p = new List<PendingContract>();
                    contract.Pendings = p;

                    var cc = new List<CourtContract>();
                    contract.CourtDates = cc;
                }

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.LegalIssuesAssessmentSection != null)
                {
                    contract.AssessmentRowVersion     = ia.LegalIssuesAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }

            return contract;
        }
    }
}
