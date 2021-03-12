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
   public class WorkProgramSectionValidator:AbstractValidator<WorkProgramSectionContract>
    {
       public WorkProgramSectionValidator()
       {
           RuleFor(x => x.WorkPrograms).SetCollectionValidator(new WorkProgramValidator());
       }
    }
}
