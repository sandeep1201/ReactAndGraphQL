using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class WagehourValidator : AbstractValidator<WageHourContract>
    {
        public WagehourValidator()
        {
            // strings limit rules are validated in syc with database not app rule based validation 
            RuleFor(x => x.CurrentPayTypeDetails).Length(0, 1000).WithMessage("Maximum characters cannot exceed 1000");
            RuleFor(x => x.WageHourHistories).SetCollectionValidator(new WageHourHistoryValidator());
        }

    }
}
