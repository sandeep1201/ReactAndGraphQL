using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
   public class WageHourHistoryValidator:AbstractValidator<WageHourHistoryContract>
    {
        // strings limit rules are validated in syc with database not app rule based validation 
        public WageHourHistoryValidator()
       {
            RuleFor(x => x.PayTypeDetails).Length(0, 1000).WithMessage("Maximum characters cannot exceed 1000");
        }
    }
}
