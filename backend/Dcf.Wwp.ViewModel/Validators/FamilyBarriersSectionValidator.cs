using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class FamilyBarriersSectionValidator : AbstractValidator<FamilyBarriersSectionContract>
    {
        public FamilyBarriersSectionValidator()
        {
            // strings limit rules are validated in syc with database not app rule based validation 
            //RuleFor(x => x.Notes).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasHealthProblemDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasHealthConcernsDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasRiskBehaviorDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasChildrenBehaviorProblemDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasChildrenSchoolExpulsionRiskDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasFamilyIssuesInhibitWorkDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasReceivingSsiDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.CaretakingResponsibilitiesDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasApplyingSsiDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.ApplicationStatusDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasOtherPersonsHelpDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasDeniedSsiDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasReceivedPastSsiDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.NoLongerReceiveSsiDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.HasInterestedInLearningDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
        }
    }
}
