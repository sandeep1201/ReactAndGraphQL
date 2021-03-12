using System;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class PostSecondaryLicenseValidator : AbstractValidator<PostSecondaryLicenseContract>
    {
        public PostSecondaryLicenseValidator()
        {
            RuleFor(x => x.Name).Length(0,120).WithMessage("Please enter not more than 120 characters");
            RuleFor(x => x.AttainedDate).Length(0, 7).WithMessage("Please enter not more than 7 characters");
        }


    }
}