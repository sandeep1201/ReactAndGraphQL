using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class LocationValidator : AbstractValidator<LocationContract>
    {
        // strings limit rules are validated in syc with database not app rule based validation 
        public LocationValidator()
        {
            RuleFor(x => x.FullAddress).Length(0, 1000).WithMessage("Maximum characters cannot exceed 100");
            RuleFor(x => x.ZipAddress).Length(0, 9).WithMessage("Maximum characters cannot exceed 9");
        }
    }
}
