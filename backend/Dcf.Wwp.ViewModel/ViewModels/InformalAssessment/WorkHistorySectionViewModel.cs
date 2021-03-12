using System;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class WorkHistorySectionViewModel : BaseInformalAssessmentViewModel
    {
        public WorkHistorySectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public WorkHistorySectionContract GetData()
        {
            return WorkHistorySectionViewModel.GetContract(InformalAssessment, Repo);
        }

        public bool PostData(WorkHistorySectionContract contract, string user)
        {
            var p = Participant;

            if (p == null)
                throw new InvalidOperationException("PIN not valid.");

            if (contract == null)
                throw new InvalidOperationException("Work History data is missing.");

            IWorkHistoryAssessmentSection whas = null;
            IWorkHistorySection           whs  = null;

            var updateTime = DateTime.Now;

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                var ia = p.InProgressInformalAssessment;

                whas = p.InProgressInformalAssessment.WorkHistoryAssessmentSection
                       ?? Repo.NewWorkHistoryAssessmentSection(ia, user);

                Repo.StartChangeTracking(whas);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                whas.ReviewCompleted = true;
                whas.ModifiedBy      = AuthUser.Username;
                whas.ModifiedDate    = updateTime;
            }

            whs = p.WorkHistorySection ?? Repo.NewWorkHistorySection(p, user);

            Repo.StartChangeTracking(whs);

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            // Employment Status
            const string ft         = "Full-Time";
            const string pt         = "Part-Time";
            const string un         = "Unemployed";
            var          fullTime   = Repo.EmploymentStatusTypeByName(ft);
            var          partTime   = Repo.EmploymentStatusTypeByName(pt);
            var          unemployed = Repo.EmploymentStatusTypeByName(un);

            whs.EmploymentStatusTypeId = contract.EmploymentStatusTypeId;

            // No matter what path the logic follows below, we'll start off marking
            // all existing WorkHistorySectionEmploymentPreventionTypeBridge records as deleted.
            foreach (var x in whs.WorkHistorySectionEmploymentPreventionTypeBridges)
            {
                x.IsDeleted = true;
            }

            // We set different things if they are Part Time or Full Time
            if (whs.EmploymentStatusTypeId == partTime?.Id || whs.EmploymentStatusTypeId == unemployed?.Id)
            {
                string preventionFactors = string.Empty;

                foreach (var pfId in contract.PreventionFactorIds)
                {
                    // Look for an existing record that's been soft deleted.
                    var exist =
                        (from x in whs.AllWorkHistorySectionEmploymentPreventionTypeBridges
                         where x.EmploymentPreventionTypeId == pfId
                         select x).SingleOrDefault();

                    if (exist != null)
                    {
                        exist.IsDeleted = false;
                    }
                    else
                    {
                        // We need a new record.
                        var bridge = Repo.NewWorkHistorySectionEmploymentPreventionTypeBridge(whs, user);
                        bridge.EmploymentPreventionTypeId = pfId;
                    }

                    preventionFactors += string.Concat(", ", Repo.EmploymentPreventionTypeById(pfId)?.Name);
                }

                whs.NonFullTimeDetails = contract.NonFullTimeDetails;

                // If we have a string, strip of the initial , and space.
                if (!String.IsNullOrWhiteSpace(preventionFactors))
                    preventionFactors = preventionFactors.Substring(2);

                whs.PreventionFactors = preventionFactors;
            }
            else
                if (whs.EmploymentStatusTypeId == fullTime?.Id)
                {
                    whs.NonFullTimeDetails = null;
                    whs.HasVolunteered     = null;
                    whs.PreventionFactors  = null;
                }

            // Set the Volunteer flag based upon employment status
            if (whs.EmploymentStatusTypeId == fullTime?.Id || whs.EmploymentStatusTypeId == partTime?.Id)
            {
                whs.HasVolunteered = null;
            }
            else
                if (whs.EmploymentStatusTypeId == unemployed?.Id)
                {
                    whs.HasVolunteered = contract.HasVolunteered;
                }

            whs.HasCareerAssessment      = contract.HasCareerAssessment;
            whs.HasCareerAssessmentNotes = string.IsNullOrWhiteSpace(contract.HasCareerAssessmentNotes) ? null : contract.HasCareerAssessmentNotes;
            whs.Notes                    = contract.Notes.SafeTrim();
            whs.ModifiedBy               = AuthUser.Username;
            whs.ModifiedDate             = updateTime;

            if (p.InProgressInformalAssessment != null)
            {
                var currentIA = Repo.GetMostRecentAssessment(p);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;
            }

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(whs, userRowVersion))
                return false;

            if (!Repo.IsRowVersionStillCurrent(whas, userAssessRowVersion))
                return false;

            // If the first save completes, it actually has already saved the LanguageAssessmentSection
            // object as well since they are on the save repository context.  But if the LanguageSection didn't
            // need saving, we still need to SaveIfChangd on the LanguageAssessmentSection.
            if (!Repo.SaveIfChanged(whs, user))
                Repo.SaveIfChanged(whas, user);

            return true;
        }

        public static WorkHistorySectionContract GetContract(IInformalAssessment ia, IRepository repo)
        {
            var contract = new WorkHistorySectionContract();

            if (ia != null)
            {
                if (ia.Participant?.WorkHistorySection != null)
                {
                    var whs = ia.Participant.WorkHistorySection;

                    contract.RowVersion   = whs.RowVersion;
                    contract.ModifiedBy   = whs.ModifiedBy;
                    contract.ModifiedDate = whs.ModifiedDate;

                    contract.EmploymentStatusTypeId   = whs.EmploymentStatusTypeId;
                    contract.EmploymentStatusTypeName = whs.EmploymentStatusType?.Name;

                    contract.NonFullTimeDetails       = whs.NonFullTimeDetails;
                    contract.HasVolunteered           = whs.HasVolunteered;
                    contract.Notes                    = whs.Notes;
                    contract.HasCareerAssessment      = whs.HasCareerAssessment;
                    contract.HasCareerAssessmentName  = whs.YesNoUnknownLookup?.Name;
                    contract.HasCareerAssessmentNotes = whs.HasCareerAssessmentNotes;

                    // Prevention Factors
                    foreach (var bridge in whs.WorkHistorySectionEmploymentPreventionTypeBridges)
                    {
                        contract.AddPreventionFactor(bridge.EmploymentPreventionTypeId,
                                                     bridge.EmploymentPreventionType.Name);
                    }
                }

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.WorkHistoryAssessmentSection != null)
                {
                    contract.AssessmentRowVersion     = ia.WorkHistoryAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }

            return contract;
        }
    }
}
