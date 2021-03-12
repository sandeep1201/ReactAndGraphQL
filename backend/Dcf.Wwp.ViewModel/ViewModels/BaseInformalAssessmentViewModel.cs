using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public abstract class BaseInformalAssessmentViewModel : BasePinViewModel
    {
        protected BaseInformalAssessmentViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        protected IInformalAssessment InformalAssessment { get; set; }

        protected override void OnParticipantLoaded()
        {
            // We will always use the most recent assessment.
            // InformalAssessment = (from x in Participant.InformalAssessments where !x.IsDeleted orderby x.CreatedDate descending select x).FirstOrDefault();  // old code

            InformalAssessment = Participant.InformalAssessments
                                            .OrderByDescending(i => i.CreatedDate)
                                            .FirstOrDefault(i => !i.IsDeleted);
        }

        protected static void UpdateRowVersionAndModifiedIfAssessmentMoreRecent(BaseInformalAssessmentContract contract, ICommonModel section, ICommonModel assessmentSection)
        {
            // If the assessment section is null, then just return.
            if (assessmentSection != null)
            {
                contract.AssessmentRowVersion = assessmentSection.RowVersion;

                // Check if the modified date is not set... if not then set it or if the assessment 
                // section  mod date is after the main section then also update it.
                if (!contract.ModifiedDate.HasValue || section.ModifiedDate < assessmentSection.ModifiedDate)
                {
                    contract.ModifiedBy   = assessmentSection.ModifiedBy;
                    contract.ModifiedDate = assessmentSection.ModifiedDate;
                }
            }
        }
    }
}
