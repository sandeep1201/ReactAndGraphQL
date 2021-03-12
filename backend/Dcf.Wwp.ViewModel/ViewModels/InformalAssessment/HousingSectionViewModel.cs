using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Cww;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Constants = Dcf.Wwp.Model.Interface.Constants;


namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class HousingSectionViewModel : BaseInformalAssessmentViewModel
    {
        public HousingSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public HousingSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        public bool PostData(HousingSectionContract contract, string user)
        {
            var p = Participant;
            if (p == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("Housing data is missing.");

            IHousingSection hs = null;
            IHousingAssessmentSection has = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                has = p.InProgressInformalAssessment.HousingAssessmentSection ??
                      Repo.NewHousingAssessmentSection(p.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(has);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                has.ReviewCompleted = true;
                has.ModifiedBy   = AuthUser.Username;
                has.ModifiedDate = updateTime;
            }

            hs = p.HousingSection ?? Repo.NewHousingSection(p, user);
            hs.ModifiedBy   = AuthUser.Username;
            hs.ModifiedDate = updateTime;

            Repo.StartChangeTracking(hs);

            var userRowVersion = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            // Current Housing Data update.
            //var housingType = repo.HousingTypeBySortOrder(md.HousingCurrentType);
            hs.HousingSituationId = contract.HousingSituationId;
            hs.CurrentHousingBeginDate = contract.CurrentHousingBeginDate?.ToDateTimeMonthYear();
            hs.CurrentHousingEndDate = contract.CurrentHousingEndDate?.ToDateTimeMonthYear();
            hs.HasCurrentEvictionRisk = contract.HasCurrentEvictionRisk;
            hs.IsCurrentAmountUnknown = contract.IsCurrentAmountUnknown;
            if (contract.IsCurrentAmountUnknown.HasValue && contract.IsCurrentAmountUnknown.Value)
            {
                hs.CurrentMonthlyAmount = null;
            }
            else
            {
                //TODO: Check if current month amount is not a number
                var amountIsNumber = CheckCurrentMonthlyAmountIfDecimal(contract.CurrentMonthlyAmount);
                hs.CurrentMonthlyAmount = (amountIsNumber) ? System.Convert.ToDecimal(contract.CurrentMonthlyAmount) : hs.CurrentMonthlyAmount = null;
            }

            hs.CurrentHousingDetails = contract.CurrentHousingDetails?.Trim() == "" ? null : contract.CurrentHousingDetails;
            hs.HasBeenEvicted = contract.HasBeenEvicted;
            hs.IsCurrentMovingToHistory = contract.IsCurrentMovingToHistory;

            // Here we soft delete histories.
            var housinghistoriesByIdModel = (from x in contract.Histories select x.Id).ToList();
            var housinghistoriesByIdDb = (from x in hs.HousingHistories select x.Id).ToList();
            DeleteFromList(hs.HousingHistories, housinghistoriesByIdModel, housinghistoriesByIdDb);

            #region Restoration HousingHistoricallist

            // list of histories from model.
            var historiesModel = contract.Histories;
            // List of all histories from Database.   
            var historiesDb = from x in hs.AllHousingHistories select x;
            var restoreList = new List<HousingHistoryContract>();
            int sortOrder = 0;
            foreach (var x in contract.Histories)
            {
                sortOrder++;
                x.SortOrder = sortOrder;
            }

            foreach (var h in historiesDb)
            {
                foreach (var m in historiesModel)
                {
                    if (h.HousingSituationId == m.HistoryType &&
                        h.BeginDate == m.BeginDate?.ToDateTimeMonthYear() &&
                        h.EndDate == m.EndDate?.ToDateTimeMonthYear() &&
                        h.IsAmountUnknown == m.IsAmountUnknown &&
                        h.HasEvicted == m.HasEvicted &&
                        h.MonthlyAmount == System.Convert.ToDecimal(m.MonthlyAmount) &&
                        h.Details == m.Details
                    )
                    {
                        h.SortOrder = m.SortOrder;
                        h.IsDeleted = false;
                        restoreList.Add(m);
                    }
                }
            }

            foreach (var r in restoreList)
            {
                contract.Histories.Remove(r);
            }

            #endregion

            foreach (var history in contract.Histories)
            {
                //if (history.Details == null)
                //{
                //    var otherhousingsituation = Repo.OtherHousingSituation(history.HistoryType);
                //    if (otherhousingsituation.Name == "Other")
                //    {
                //        throw new InvalidOperationException("If other is Selected Details cannot be Empty");
                //    }
                //}
                IHousingHistory hh = null;
                hh = hs.HousingHistories.SingleOrDefault(x => x.Id == history.Id && history.Id != 0);
                if (hh == null || hh.Id == 0)
                {
                    hh = Repo.NewHousingHistory(hs, user);
                }
                //sortOrder++;

                hh.HousingSituationId = history.HistoryType;
                hh.SortOrder = history.SortOrder;
                hh.BeginDate = history.BeginDate?.ToDateTimeMonthYear();
                hh.EndDate = history.EndDate?.ToDateTimeMonthYear();
                hh.HasEvicted = history.HasEvicted;
                hh.IsAmountUnknown = history.IsAmountUnknown;
                if (history.IsAmountUnknown.HasValue && history.IsAmountUnknown.Value)
                {
                    hh.MonthlyAmount = null;
                }
                else
                {
                    hh.MonthlyAmount = System.Convert.ToDecimal(history.MonthlyAmount);
                }
                hh.Details = history.Details;
                hh.ModifiedDate = updateTime;
                hh.ModifiedBy = user;
                // TODO: Do not need this statement because we pass parent object when newing housing history.
                hs.HousingHistories.Add(hh);
            }

            // Current housing Utility Update.
            hs.HasUtilityDisconnectionRisk = contract.HasUtilityDisconnectionRisk;
            if (hs.HasUtilityDisconnectionRisk.HasValue && hs.HasUtilityDisconnectionRisk.Value)
            {
                hs.UtilityDisconnectionRiskNotes = contract.UtilityDisconnectionRiskNotes;
            }
            else if (hs.HasUtilityDisconnectionRisk.HasValue && !hs.HasUtilityDisconnectionRisk.Value)
            {
                hs.UtilityDisconnectionRiskNotes = null;
            }

            //Current Housing situation participation
            hs.HasDifficultyWorking = contract.HasDifficultyWorking;
            if (hs.HasDifficultyWorking.HasValue && hs.HasDifficultyWorking.Value)
            {
                hs.DifficultyWorkingNotes = contract.DifficultyWorkingNotes;
            }
            else if (hs.HasDifficultyWorking.HasValue && !hs.HasDifficultyWorking.Value)
            {
                hs.DifficultyWorkingNotes = null;
            }

            hs.Notes = contract.HousingNotes;

            var currentIA = Repo.GetMostRecentAssessment(p);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(hs, userRowVersion))
                return false;

            if (!Repo.IsRowVersionStillCurrent(has, userAssessRowVersion))
                return false;

            // If the first save completes, it actually has already saved the AssessmentSection
            // object as well since they are on the save repository context.  But if the Section didn't
            // need saving, we still need to SaveIfChangd on the AssessmentSection.
            if (!Repo.SaveIfChanged(hs, user))
                Repo.SaveIfChanged(has, user);

            return true;
        }

        private static bool DeleteFromList(ICollection<IHousingHistory> list, List<int> modelIds, List<int> dbIds)
        {
            var deletedList = dbIds.Except(modelIds).ToArray();

            foreach (int n in deletedList)
            {
                var c = list.First(x => x.Id == n);
                c.IsDeleted = true;
            }
            return true;
        }

        public static HousingSectionContract GetContract(IInformalAssessment ia, IParticipant part, IRepository repo)
        {
            var contract = new HousingSectionContract();
            var currentAddressDetails = repo.CwwCurrentAddressDetails(part.PinNumber.ToString());

            // Only show CWW if we have the Address.
            // Without the Address it's bad data.
            if (currentAddressDetails?.Address != null)
            {
                contract.CwwHousing = new List<CwwHousing>();
                var cwwH = new CwwHousing();
                // Set the CWW contract properties for current address.
                cwwH.Address = currentAddressDetails.Address;
                cwwH.State = currentAddressDetails.State;
                cwwH.City = currentAddressDetails.City;
                cwwH.Zip = currentAddressDetails.Zip;
                cwwH.Subsidized = currentAddressDetails.IsSubsidized.ToYesNoNonNull();
                cwwH.RentObligation = currentAddressDetails.ShelterAmount?.ToString().AddDollarSign();
                contract.CwwHousing.Add(cwwH);
            }
            else
            {
                var cwwh = new List<CwwHousing>();
                contract.CwwHousing = cwwh;
            }

            // Since the action needed is not dependent upon any section or assessment section data,
            // we'll add it right away.
            contract.ActionNeeded = ActionNeededViewModel.GetActionNeededContract(part, repo,
                Constants.ActionNeededPage.Housing);

            if (ia != null)
            {
                if (ia.Participant?.HousingSection != null)
                {
                    var hdb = ia.Participant.HousingSection;

                    contract.RowVersion = hdb.RowVersion;
                    contract.ModifiedBy = hdb.ModifiedBy;
                    contract.ModifiedDate = hdb.ModifiedDate;
                    contract.HousingSituationId = hdb.HousingSituationId;
                    contract.HousingSituationName = hdb.HousingSituation?.Name.SafeTrim();
                    contract.CurrentHousingBeginDate = hdb.CurrentHousingBeginDate?.ToString("MM/yyyy");
                    contract.CurrentHousingEndDate = hdb.CurrentHousingEndDate?.ToString("MM/yyyy");
                    contract.HasCurrentEvictionRisk = hdb.HasCurrentEvictionRisk;
                    contract.IsCurrentAmountUnknown = hdb.IsCurrentAmountUnknown;
                    if (hdb.IsCurrentAmountUnknown.HasValue && hdb.IsCurrentAmountUnknown.Value)
                        contract.CurrentMonthlyAmount = null;
                    else
                        contract.CurrentMonthlyAmount = hdb.CurrentMonthlyAmount?.ToString();
                    contract.CurrentHousingDetails = hdb.CurrentHousingDetails;
                    contract.HasBeenEvicted = hdb.HasBeenEvicted;
                    // Map the nullable bool to a non-nullable bool.
                    contract.IsCurrentMovingToHistory = hdb.IsCurrentMovingToHistory.HasValue && hdb.IsCurrentMovingToHistory.Value;

                    // Here it gets list  housing histories. 
                    var housingHistories = new List<HousingHistoryContract>();
                    foreach (var h in hdb.HousingHistories.OrderByDescending(x => x.BeginDate).ToList())
                    {
                        var model = new HousingHistoryContract();
                        model.HistoryType = h.HousingSituationId;
                        model.HistoryTypeName = h.HousingSituation?.Name.SafeTrim();
                        ;
                        model.BeginDate = h.BeginDate?.ToString("MM/yyyy");
                        model.EndDate = h.EndDate?.ToString("MM/yyyy");
                        model.HasEvicted = h.HasEvicted;
                        model.MonthlyAmount = h.MonthlyAmount?.ToString();
                        model.IsAmountUnknown = h.IsAmountUnknown;
                        model.Details = h.Details;
                        model.Id = h.Id;
                        housingHistories.Add(model);
                    }
                    contract.Histories = housingHistories;

                    contract.HasUtilityDisconnectionRisk = hdb.HasUtilityDisconnectionRisk;
                    contract.UtilityDisconnectionRiskNotes = hdb.UtilityDisconnectionRiskNotes;

                    contract.HasDifficultyWorking = hdb.HasDifficultyWorking;
                    contract.DifficultyWorkingNotes = hdb.DifficultyWorkingNotes;

                    contract.HousingNotes = hdb.Notes;
                }
                else
                {
                    // Normal contract property setting when their exist no housing section data in back end.
                    var hhs = new List<HousingHistoryContract>();
                    contract.Histories = hhs;
                }

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.HousingAssessmentSection != null)
                {
                    contract.AssessmentRowVersion = ia.HousingAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }
            return contract;
        }

        private bool CheckCurrentMonthlyAmountIfDecimal(string monthlyAmount)
        {
            decimal result;
            if (decimal.TryParse(monthlyAmount, out result))
            {
                return true;
            }

            return false;
        }
    }
}
