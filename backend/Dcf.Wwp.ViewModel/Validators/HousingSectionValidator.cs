using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class HousingSectionValidator : AbstractValidator<HousingSectionContract>
    {
        public HousingSectionValidator()
        {
            //RuleFor(x => x.CurrentHousingMonths)
            //    .Must(duration => duration > 999)
            //    .WithMessage("Duration cannot be greater than 999");
            //RuleFor(x => x.CurrentHousingRent)
            //    .Must(housingcurrentrent => housingcurrentrent.ToInt() >= 0 && housingcurrentrent.ToInt() <= 99999.99)
            //    .WithMessage("Duration cannot be greater than 999");
            // strings limit rules are validated in syc with database not app rule based validation 
            RuleFor(x => x.CurrentHousingDetails).Length(0, 1000)
                .WithMessage("Current Housing Notes cannot exceed maximum characters of 1000");          
            RuleFor(x => x.Histories).SetCollectionValidator(new HousingHistoriesValidator());

        }
    }
}
