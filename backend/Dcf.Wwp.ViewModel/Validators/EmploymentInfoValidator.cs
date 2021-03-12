using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
   public class EmploymentInfoValidator:AbstractValidator<EmploymentInfoContract>
    {
        // strings limit rules are validated in syc with database not app rule based validation 
        public EmploymentInfoValidator()
       {
            RuleFor(x => x.Notes).Length(0, 1000).WithMessage("Maximum characters cannot exceed 1000");
            RuleFor(x => x.JobPosition).Length(0, 50).WithMessage("Maximum characters cannot exceed 50");
            RuleFor(x => x.CompanyName).Length(0, 100).WithMessage("Maximum characters cannot exceed 100");
            RuleFor(x => x.LeavingReasonDetails).Length(0, 1000).WithMessage("Maximum characters cannot exceed 1000");
            RuleFor(x => x.ExpectedScheduleDetails).Length(0, 1000).WithMessage("Maximum characters cannot exceed 1000");
            RuleFor(x => x.WorkerId).Length(0, 120).WithMessage("Maximum characters cannot exceed 50");
            RuleFor(x => x.JobFoundMethodDetails).Length(0, 500).WithMessage("Maximum characters cannot exceed 500");
           RuleFor(x => x.WageHour).SetValidator(new WagehourValidator());
            RuleFor(x => x.Location).SetValidator(new LocationValidator());


       }
    }
}
