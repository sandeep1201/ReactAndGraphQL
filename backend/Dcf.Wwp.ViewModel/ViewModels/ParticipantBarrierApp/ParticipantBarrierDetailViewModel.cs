using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.ParticipantBarrierApp
{
    public class ParticipantBarrierDetailViewModel : BasePinViewModel
    {
        private readonly IAuthUser          _authUser;
        private readonly ITransactionDomain _transactionDomain;

        public ParticipantBarrierDetailViewModel(IRepository repo, IAuthUser authUser, ITransactionDomain transactionDomain) : base(repo, authUser)
        {
            _authUser          = authUser;
            _transactionDomain = transactionDomain;
        }

        public IEnumerable<BarrierDetailContract> GetParticipantBarrierLists()
        {
            var activeBarrierDetails = Participant.AllBarrierDetails
                                                  .Select(GetParticipantBarrierContract)
                                                  .ToList();

            return (activeBarrierDetails);
        }

        private BarrierDetailContract GetParticipantBarrierContract(IBarrierDetail bd)
        {
            BarrierDetailContract barrierDetailInfo = null;

            if (bd != null)
            {
                barrierDetailInfo = new BarrierDetailContract { Id = bd.Id };

                var barrierSubType = new BarrierSubTypeContract
                                     {
                                         BarrierSubTypeNames = new List<string>(),
                                         BarrierSubTypes     = new List<int>()
                                     };

                // We need to look at the ActionBridge table to get the list
                // of action needed ID's that the user has previously chosen.
                if (bd.NonDeletedBarrierTypeBarrierSubTypeBridges != null)
                {
                    foreach (var actionBridge in bd.NonDeletedBarrierTypeBarrierSubTypeBridges)
                    {
                        if (actionBridge.BarrierSubTypeId.HasValue)
                        {
                            barrierSubType.BarrierSubTypes.Add(actionBridge.BarrierSubTypeId.Value);
                        }

                        barrierSubType.BarrierSubTypeNames.Add(actionBridge.BarrierSubtype?.Name);
                    }
                }

                barrierDetailInfo.BarrierSubType           = barrierSubType;
                barrierDetailInfo.BarrierTypeId            = bd.BarrierTypeId;
                barrierDetailInfo.BarrierTypeName          = bd.BarrierType.Name;
                barrierDetailInfo.OnsetDate                = bd.OnsetDate.ToStringMonthYear();
                barrierDetailInfo.EndDate                  = bd.EndDate.ToStringMonthYear();
                barrierDetailInfo.WasClosedAtDisenrollment = bd.WasClosedAtDisenrollment;
                barrierDetailInfo.Details                  = bd.Details;
                barrierDetailInfo.IsConverted              = bd.IsConverted;
                barrierDetailInfo.RowVersion               = bd.RowVersion;
                barrierDetailInfo.IsDeleted                = bd.IsDeleted;
                barrierDetailInfo.ModifiedBy               = bd.ModifiedBy;
                barrierDetailInfo.ModifiedDate             = bd.ModifiedDate;

                //Get Formal Assessments based on the isDeleted flag on BarrierDetail
                barrierDetailInfo.FormalAssessments = new List<FormalAssessmentContract>();
                var fas = bd.FormalAssessments?.AsQueryable();

                if (!bd.IsDeleted)
                {
                    fas = fas.Where(i => i.DeleteReasonId == null);
                }

                var formalAssessments = fas.ToList();

                foreach (var fa in formalAssessments)
                {
                    barrierDetailInfo.FormalAssessments?.Add(new FormalAssessmentContract
                                                             {
                                                                 Id                                         = fa.Id,
                                                                 ReferralDate                               = fa.ReferralDate.ToStringMonthDayYear(),
                                                                 ReferralDeclined                           = fa.ReferralDeclined,
                                                                 ReferralDetails                            = fa.ReferralDetails,
                                                                 AssessmentDate                             = fa.AssessmentDate.ToStringMonthDayYear(),
                                                                 AssessmentNotCompleted                     = fa.AssessmentNotCompleted,
                                                                 AssessmentDetails                          = fa.AssessmentDetails,
                                                                 SymptomId                                  = fa.SymptomId,
                                                                 SymptomName                                = fa.Symptom?.Name,
                                                                 ReassessmentRecommendedDate                = fa.ReassessmentRecommendedDate.ToStringMonthDayYear(),
                                                                 SymptomDetails                             = fa.SymptomDetails,
                                                                 AssessmentProviderContactId                = fa.AssessmentProviderContactId,
                                                                 HoursParticipantCanParticipate             = fa.HoursParticipantCanParticipate,
                                                                 HoursParticipantCanParticipateDetails      = fa.HoursParticipantCanParticipateDetails,
                                                                 HoursParticipantCanParticipateIntervalId   = fa.HoursParticipantCanParticipateIntervalId,
                                                                 HoursParticipantCanParticipateIntervalDesc = fa.IntervalType?.Name,
                                                                 IsRecommendedDateNotNeeded                 = fa.IsRecommendedDateNotNeeded
                                                             });
                }

                //Get Barrier Accommodations based on the isDeleted flag on BarrierDetail
                barrierDetailInfo.IsAccommodationNeeded = bd.IsAccommodationNeeded;
                barrierDetailInfo.BarrierAccommodations = new List<BarrierAccommodationContract>();
                var bas = bd.BarrierAccommodations?.AsQueryable();

                if (!bd.IsDeleted)
                {
                    bas = bas.Where(i => i.DeleteReasonId == null);
                }

                var barrierAccommodations = bas.ToList();

                foreach (var ba in barrierAccommodations)
                {
                    barrierDetailInfo.BarrierAccommodations?.Add(new BarrierAccommodationContract
                                                                 {
                                                                     Id                = ba.Id,
                                                                     AccommodationId   = ba.AccommodationId,
                                                                     AccommodationName = ba.Accommodation?.Name,
                                                                     BeginDate         = ba.BeginDate.ToStringMonthDayYear(),
                                                                     EndDate           = ba.EndDate.ToStringMonthDayYear(),
                                                                     Details           = ba.Details
                                                                 });
                }

                barrierDetailInfo.Contacts = new List<int?>();

                // We need to look at the ContactBridge table to get the list of contact
                // ID's that the user has previously chosen.
                if (bd.BarrierDetailContactBridges != null)
                {
                    foreach (var contact in bd.BarrierDetailContactBridges.Where(i => !i.IsDeleted))
                    {
                        if (contact.ContactId.HasValue)
                        {
                            barrierDetailInfo.Contacts.Add(contact.ContactId.Value);
                        }
                    }
                }
            }

            return barrierDetailInfo;
        }

        public BarrierDetailContract GetParticipantBarrierInfo(int id)
        {
            var bd = Participant.AllBarrierDetails.FirstOrDefault(i => i.Id == id);

            var r = GetParticipantBarrierContract(bd);

            return  (r);
        }

        public UpsertResponse<IBarrierDetail> UpsertData(BarrierDetailContract contract, int id, string pin, string user)
        {
            var p = Repo.GetParticipant(pin);

            if (p == null)
            {
                throw new InvalidOperationException("Pin not valid.");
            }

            if (contract == null)
            {
                throw new InvalidOperationException("Barrier Detail data is missing.");
            }

            IBarrierDetail barrierDetail = null;
            barrierDetail = id != 0 ? Repo.BarrierDetailById(id) : Repo.NewBarrierDetailInfo(p, user);

            Repo.StartChangeTracking(barrierDetail);
            barrierDetail.BarrierTypeId = contract.BarrierTypeId;
            var userRowVersion   = contract.RowVersion;
            var barrierType      = Repo.BarrierTypeById(contract.BarrierTypeId);
            var modificationDate = DateTime.Now; // We want one modification date so that the section and the children have the same timestamp.

            #region Transaction

            var officeId = p.EnrolledParticipantEnrolledPrograms
                            .Where(i => _authUser.Authorizations.Where(j => j.StartsWith("canAccessProgram_"))
                                                 .Select(j => j.Trim().ToLower().Split('_')[1])
                                                 .Contains(i.EnrolledProgram.ProgramCode.Trim().ToLower())
                                        && i.Office.ContractArea.Organization.EntsecAgencyCode.Trim().ToLower() == _authUser.AgencyCode.Trim().ToLower())
                            .OrderByDescending(i => i.EnrollmentDate)
                            .First().Office.Id;

            var transactionContract = new TransactionContract
            {
                ParticipantId = p.Id,
                WorkerId = Repo.WorkerByWIUID(_authUser.WIUID).Id,
                OfficeId = officeId,
                EffectiveDate = modificationDate,
                CreatedDate = modificationDate,
                TransactionTypeCode = TransactionTypes.ParticipantBarrierAdded,
                ModifiedBy = _authUser.WIUID
            };

            var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

            if (transaction != null && barrierDetail.Id == 0)
                Repo.NewTransaction(transaction as ITransaction);

            if (!string.IsNullOrWhiteSpace(contract.EndDate) && barrierDetail.EndDate == null)
            {
                transactionContract.TransactionTypeCode = TransactionTypes.ParticipantBarrierEnded;

                var transactionPbEnd = _transactionDomain.InsertTransaction(transactionContract, true);

                if (transactionPbEnd != null)
                    Repo.NewTransaction(transactionPbEnd as ITransaction);
            }

            #endregion


            barrierDetail.OnsetDate                = contract.OnsetDate.ToDateTimeMonthYear();
            barrierDetail.EndDate                  = contract.EndDate.ToDateTimeMonthYear();
            barrierDetail.WasClosedAtDisenrollment = contract.WasClosedAtDisenrollment;
            barrierDetail.Details                  = contract.Details;
            barrierDetail.IsConverted              = contract?.IsConverted;

            // Formal Assessments repeaters

            #region Formal Assessments repeaters

            // Grab all the Formal Assessments records from the database which includes the soft deleted item.                
            var       allFormalAssessments = barrierDetail.FormalAssessments.ToList();
            List<int> formalAssessmentsIds;

            // The “Formal Assessment” section of “Add View” is displayed unless “Domestic Violence” is selected for “Barrier Type”. 
            if (barrierType?.Name == BarrierType.DomesticViolence)
            {
                // The simplest thing to do if the posted contract data indicates there should
                // be no Formal Assessments  is to just clear out any incoming records.
                // That will allow the normal logic below to just mark any existing records as
                // deleted, even if they were posted in the data contract.
                contract.FormalAssessments = null;

                // further down, so we will need up an empty list since there aren't any.                   
                formalAssessmentsIds = new List<int>();

                if (barrierDetail.Id != 0)
                {
                    Repo.DeleteFormalAssements(formalAssement => formalAssement.BarrierDetailsId == barrierDetail.Id);
                }
            }
            else
            {
                // Now, cleanse the FormalAssessments incoming data.  This means clearing out empty repeater items.
                contract.FormalAssessments = contract.FormalAssessments.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                contract.FormalAssessments.UpdateNewItemsIfSimilarToExisting(allFormalAssessments, FormalAssessmentContract.AdoptIfSimilarToModel);

                // Get the Id's of the FamilyMemberContract records that are not new.
                //formalAssessmentsIds = (from x in contract.FormalAssessments where x.Id != 0 select x.Id).ToList();
                formalAssessmentsIds = contract.FormalAssessments.Where(i => i.Id != 0).Select(i => i.Id).ToList();
            }

            // At this point we have the model collections cleaned up and ready to mark the unused items as deleted.                          
            // Start with getting the active formal Assessment IDs.
            // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
            // to mark the unused items as deleted.

            var formalAssessmentsPostedData = formalAssessmentsIds.ToList();
            //allFormalAssessments.MarkUnusedItemsAsDeleted(formalAssessmentsPostedData);

            if (contract.DeletedFormalAssessments != null)
            {
                foreach (var dba in contract.DeletedFormalAssessments)
                {
                    var deletedFa = barrierDetail.FormalAssessments.FirstOrDefault(x => x.Id == dba.Id);

                    if (deletedFa != null)
                    {
                        deletedFa.DeleteReasonId = dba.DeleteReasonId;
                    }
                }
            }

            // variable used in looping logic below
            IFormalAssessment fa;

            // Now update the database items with the posted model data.
            if (contract.FormalAssessments != null && contract.FormalAssessments.Count > 0)
            {
                foreach (var fmas in contract.FormalAssessments)
                {
                    if (fmas.IsNew())
                    {
                        fa              = Repo.NewFormalAssessment(barrierDetail);
                        fa.ModifiedDate = modificationDate;
                        fa.ModifiedBy   = user;
                    }
                    else
                    {
                        fa = (from x in allFormalAssessments where x.Id == fmas.Id select x).SingleOrDefault();
                    }

                    Debug.Assert(fa != null, "IFamilyMember should not be null.");

                    fa.ReferralDeclined = fmas.ReferralDeclined;
                    fa.ReferralDetails  = fmas.ReferralDetails;

                    // A bunch of fields don't show if it's declined.
                    if (fmas.ReferralDeclined.HasValue && fmas.ReferralDeclined.Value)
                    {
                        fa.ReferralDate                             = null;
                        fa.SymptomId                                = null;
                        fa.ReassessmentRecommendedDate              = null;
                        fa.IsRecommendedDateNotNeeded               = null;
                        fa.AssessmentDate                           = null;
                        fa.AssessmentDetails                        = null;
                        fa.AssessmentNotCompleted                   = null;
                        fa.AssessmentProviderContactId              = null;
                        fa.HoursParticipantCanParticipate           = null;
                        fa.HoursParticipantCanParticipateDetails    = null;
                        fa.HoursParticipantCanParticipateIntervalId = null;
                    }
                    else
                    {
                        fa.ReferralDate           = fmas.ReferralDate.ToDateTimeMonthDayYear();
                        fa.AssessmentDetails      = fmas.AssessmentDetails;
                        fa.AssessmentNotCompleted = fmas.AssessmentNotCompleted;

                        if (fmas.AssessmentNotCompleted.HasValue && fmas.AssessmentNotCompleted.Value)
                        {
                            fa.SymptomId                                = null;
                            fa.ReassessmentRecommendedDate              = null;
                            fa.IsRecommendedDateNotNeeded               = null;
                            fa.AssessmentDate                           = null;
                            fa.AssessmentProviderContactId              = null;
                            fa.SymptomDetails                           = null;
                            fa.HoursParticipantCanParticipate           = null;
                            fa.HoursParticipantCanParticipateDetails    = null;
                            fa.HoursParticipantCanParticipateIntervalId = null;
                        }
                        else
                        {
                            fa.SymptomId                  = fmas.SymptomId;
                            fa.AssessmentDate             = fmas.AssessmentDate.ToDateTimeMonthDayYear();
                            fa.IsRecommendedDateNotNeeded = fmas.IsRecommendedDateNotNeeded;

                            if (fmas.IsRecommendedDateNotNeeded.HasValue && fmas.IsRecommendedDateNotNeeded.Value)
                            {
                                fa.ReassessmentRecommendedDate = null;
                            }
                            else
                            {
                                fa.ReassessmentRecommendedDate = fmas.ReassessmentRecommendedDate.ToDateTimeMonthDayYear();
                            }

                            fa.AssessmentNotCompleted      = fmas.AssessmentNotCompleted;
                            fa.AssessmentProviderContactId = fmas.AssessmentProviderContactId;
                            fa.SymptomDetails              = fmas.SymptomDetails;

                            // Null dependent fields if AssessmentDate is null or empty
                            if (fmas.AssessmentDate.IsNullOrEmpty())
                            {
                                fa.SymptomId                   = null;
                                fa.ReassessmentRecommendedDate = null;
                                fa.IsRecommendedDateNotNeeded  = null;
                                fa.SymptomDetails              = null;
                            }

                            fa.HoursParticipantCanParticipate           = fmas.HoursParticipantCanParticipate;
                            fa.HoursParticipantCanParticipateDetails    = fmas.HoursParticipantCanParticipateDetails;
                            fa.HoursParticipantCanParticipateIntervalId = fmas.HoursParticipantCanParticipateIntervalId;
                        }
                    }

                    fa.DeleteReasonId = null; // Need to set this in case we've un-soft-deleted an item up above.
                }
            }

            #endregion

            barrierDetail.IsAccommodationNeeded = contract.IsAccommodationNeeded;
            // Accomodation repeaters           

            #region Barrier Accommodation repeaters

            // Grab all the Barrier Accommodation records from the database which includes the soft deleted item.                
            var       allBarrierAccommodations = barrierDetail.BarrierAccommodations.ToList();
            List<int> barrierAccommodationsIds;

            // The “Barrier Accommodations” section of “Add View” is displayed if Yes is chosen.
            if (contract.IsAccommodationNeeded.HasValue && contract.IsAccommodationNeeded.Value)
            {
                // Now, cleanse the Barrier Accommodations incoming data.  This means clearing out empty repeater items.
                contract.BarrierAccommodations = contract.BarrierAccommodations.WithoutEmpties();

                // Next map any new items that are similar to existing/deleted items.
                contract.BarrierAccommodations.UpdateNewItemsIfSimilarToExisting(allBarrierAccommodations,
                                                                                 BarrierAccommodationContract.AdoptIfSimilarToModel);

                // Get the Id's of the BarrierAccommodationsContract records that are not new.
                barrierAccommodationsIds = (from x in contract.BarrierAccommodations where x.Id != 0 select x.Id).ToList();
            }
            else
            {
                // The simplest thing to do if the posted contract data indicates there should
                // be no Barrier Accommodations  is to just clear out any incoming records.
                // That will allow the normal logic below to just mark any existing records as
                // deleted, even if they were posted in the data contract.
                contract.BarrierAccommodations = null;

                // further down, so we will need up an empty list since there aren't any.
                barrierAccommodationsIds = new List<int>();
            }

            // At this point we have the model collections cleaned up and ready to mark the unused
            // items as deleted.
            // Start with getting the active Barrier Accommodations IDs.
            // Now we have our list of active (in-use) Id's.  We'll use our handy extension method
            // to mark the unused items as deleted.

            var barrierAccommodationsPostedData = barrierAccommodationsIds.ToList();
            //allBarrierAccommodations.MarkUnusedItemsAsDeleted(barrierAccommodationsPostedData);

            // Set Delete Reason Ids for delete Data.
            if (contract.DeletedBarrierAccommodations != null)
            {
                foreach (var dba in contract.DeletedBarrierAccommodations)
                {
                    var deletedAc = barrierDetail.BarrierAccommodations.FirstOrDefault(x => x.Id == dba.Id);

                    if (deletedAc != null)
                    {
                        deletedAc.DeleteReasonId = dba.DeleteReasonId;
                    }
                }
            }

            // variable used in looping logic below
            IBarrierAccommodation ba;

            // Now update the database items with the posted model data.
            if (contract.BarrierAccommodations != null)
            {
                foreach (var bas in contract.BarrierAccommodations)
                {
                    if (bas.IsNew())
                    {
                        ba              = Repo.NewBarrierAccommodation(barrierDetail);
                        ba.ModifiedDate = modificationDate;
                        ba.ModifiedBy   = _authUser.Username;
                    }
                    else
                    {
                        ba = (from x in allBarrierAccommodations where x.Id == bas.Id select x).SingleOrDefault();
                    }

                    Debug.Assert(ba != null, "IBarrierAccomodation should not be null.");

                    ba.AccommodationId = bas.AccommodationId;
                    ba.BeginDate       = bas.BeginDate.ToDateTimeMonthDayYear();
                    ba.EndDate         = bas.EndDate.ToDateTimeMonthDayYear();
                    ba.Details         = bas.Details;
                    ba.DeleteReasonId  = null; // Need to set this in case we've un-soft-deleted an item up above.
                    ba.ModifiedDate    = modificationDate;
                    ba.ModifiedBy      = _authUser.Username;
                }
            }

            #endregion

            // Mark all Barrier SubTypes deleted.
            foreach (var x in barrierDetail.NonDeletedBarrierTypeBarrierSubTypeBridges)
            {
                x.IsDeleted = true;
            }

            if (contract.BarrierSubType?.BarrierSubTypes != null)
            {
                foreach (var bst in contract.BarrierSubType.BarrierSubTypes)
                {
                    var restore = barrierDetail.BarrierTypeBarrierSubTypeBridges?.FirstOrDefault(z => z.BarrierSubTypeId == bst);

                    if (restore != null)
                    {
                        restore.ModifiedDate = modificationDate;
                        restore.ModifiedBy   = user;
                        restore.IsDeleted    = false;
                    }
                    else
                    {
                        IBarrierTypeBarrierSubTypeBridge ibsub = null;
                        ibsub                  = Repo.NewBarrierTypeBarrierSubTypeBridge(barrierDetail, user);
                        ibsub.BarrierSubTypeId = bst;
                        barrierDetail.BarrierTypeBarrierSubTypeBridges.Add(ibsub);
                    }
                }
            }

            // Contacts

            foreach (var x in barrierDetail.NonBarrierDetailContactBridges)
            {
                x.IsDeleted = true;
            }

            if (contract.Contacts != null)
            {
                var contacts = contract.Contacts;

                if (contacts.Count == 0)
                {
                    //var restores = bd.BarrierTypeBarrierSubTypeBridges;
                    //foreach (var r in restores)
                    //{
                    //    {
                    //        r.IsDeleted = true;
                    //    }
                    //}
                }
                else
                {
                    foreach (var x in contacts)
                    {
                        var restore = barrierDetail.BarrierDetailContactBridges?.FirstOrDefault(z => z.ContactId == x);

                        if (restore != null)
                        {
                            restore.ModifiedDate = DateTime.Now;
                            restore.ModifiedBy   = user;
                            restore.IsDeleted    = false;
                        }
                        else
                        {
                            IBarrierDetailContactBridge ibdcb = null;
                            ibdcb           = Repo.NewBarrierDetailContactBridge(barrierDetail, user);
                            ibdcb.ContactId = x;
                            barrierDetail.BarrierDetailContactBridges.Add(ibdcb);
                        }
                    }
                }
            }

            var response = new UpsertResponse<IBarrierDetail> { UpdatedModel = barrierDetail };

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(barrierDetail, userRowVersion))
            {
                response.HasConcurrencyError = true;
            }

            Repo.SaveIfChanged(barrierDetail, user);

            return response;
        }

        public bool DeleteParticipantBarrier(int id, string user)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var participantBarrierDetailInfo = (from x in Participant.AllBarrierDetails where x.Id == id select x).SingleOrDefault();

            if (participantBarrierDetailInfo != null && participantBarrierDetailInfo.IsDeleted == false)
            {
                participantBarrierDetailInfo.IsDeleted    = true;
                participantBarrierDetailInfo.ModifiedDate = DateTime.Now;

                // TODO: Fix Sprint 15
                /*
                if (employmentInfo.JobBenefitsOfferedActionBridges != null)
                {
                    foreach (var jbodb in employmentInfo.JobBenefitsOfferedActionBridges)
                    {
                        jbodb.IsDeleted = true;
                        jbodb.ModifiedBy = user;
                    }
                }
                */
                if (participantBarrierDetailInfo.BarrierAccommodations != null)
                {
                    foreach (var ba in participantBarrierDetailInfo.BarrierAccommodations)
                    {
                        ba.DeleteReasonId = DeleteReasonLookup.BarrierDeleted;
                        ba.ModifiedBy     = user;
                        ba.ModifiedDate   = DateTime.Now;
                    }
                }

                if (participantBarrierDetailInfo.FormalAssessments != null)
                {
                    foreach (var x in participantBarrierDetailInfo.FormalAssessments)
                    {
                        x.DeleteReasonId = DeleteReasonLookup.BarrierDeleted;
                        x.ModifiedBy     = user;
                        x.ModifiedDate   = DateTime.Now;
                    }
                }

                participantBarrierDetailInfo.ModifiedBy   = user;
                participantBarrierDetailInfo.ModifiedDate = DateTime.Now;
                Repo.Save();

                return true;
            }

            return false;
        }
    }
}
