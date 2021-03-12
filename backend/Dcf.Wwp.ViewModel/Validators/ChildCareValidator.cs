using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class ChildCareValidator : AbstractValidator<ChildCareContract>
    {
        public ChildCareValidator()
        {
            // strings limit rules are validated in syc with database not app rule based validation 
            RuleFor(x => x.Details).Length(0, 1000).WithMessage("Maximum characters cannot exceed 250");
           
        }
    }
}
