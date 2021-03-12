using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class LegalIssueSectionValidator : AbstractValidator<LegalIssuesSectionContract>
    {
        public LegalIssueSectionValidator()
        {
            RuleFor(x => x.Notes).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
           // RuleFor(x => x.ActionNeeded.AssistDetails).Length(0, 380).WithMessage("Maximum Characters Cannot exceed 380");
            RuleFor(x => x.ChildSupportDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            RuleFor(x => x.CommunitySupervisonDetails).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            RuleFor(x => x.FamilyLegalIssueNotes).Length(0, 1000).WithMessage("Maximum Characters Cannot exceed 1000");
            //RuleFor(x => x.ChildSupportAmount).Cascade(CascadeMode.StopOnFirstFailure)
            //    .Must(amount => amount.ToInt() >= 0 && amount.ToInt() <= 99999.99)
            //    .WithMessage( "Amount should be in between 0 and 99,999.99, also cannot enter more than two digits after decimal");
            //RuleFor(x => x.ChildSupportAmount)
            //    .Null()
            //    .When(x => x.IsAmountUnknown.FirstOrDefault().Equals(true))
            //    .WithMessage("Date is Empty when Unknown is Selected");
            RuleFor(x => x.Convictions).SetCollectionValidator(new ConvictionValidator());
            RuleFor(x => x.CourtDates).SetCollectionValidator(new CourtDatesValidator());
            RuleFor(x => x.Pendings).SetCollectionValidator(new PendingsValidator());
        }
    }
}
