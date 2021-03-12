using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class PostSecondarySectionValidator : AbstractValidator<PostSecondarySectionContract>
    {
        public PostSecondarySectionValidator()
        {
            RuleFor(x => x.PostSecondaryColleges).SetCollectionValidator(new PostSecondaryCollegeValidator());
            RuleFor(x => x.PostSecondaryDegrees).SetCollectionValidator(new PostSecondaryDegreeValidator());
            RuleFor(x => x.PostSecondaryLicenses).SetCollectionValidator(new PostSecondaryLicenseValidator());
        }

    }
}
