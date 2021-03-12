using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class WorkProgramValidator : AbstractValidator<WorkProgramContract>
    {
        public WorkProgramValidator()
        {
          //  RuleFor(x => x.Details).Length(0, 380).WithMessage("Please enter not more than 120 characters");
          ////  RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("Start date should be prior to End date");
          //  RuleFor(x => x.StartDate).Must(PriorToCurrentDate).When(WorkStatusIfPast).WithMessage("Start date must be prior to current date  when  work status is selected past");
          //  RuleFor(x => x.EndDate).Must(PriorToCurrentDate).When(WorkStatusIfPast).WithMessage("End date cannot be prior to current date when  work status is selected past");
          //  RuleFor(x => x.StartDate).Must(PriorToCurrentDate).When(WorkStatusIfCurrent).WithMessage("Start date cannot be prior to current date  when  work status is selected present");
          //  RuleFor(x => x.EndDate).Must(AfterCurrentDate).When(WorkStatusIfCurrent).WithMessage("End date cannot be prior to current date when  work status is selected past");

        }

        private bool WorkStatusIfPast(WorkProgramContract arg)
        {
            if (arg.WorkStatus == 1)
                return true;           
            
            return false;
        }

        private bool WorkStatusIfCurrent(WorkProgramContract arg)
        {
            if (arg.WorkStatus == 2)
                return true;

            return false;
        }



        public bool PriorToCurrentDate(string startDate)
        {
            var startdatetime = startDate.ToDateTimeMonthYear();
            if (startdatetime < DateTime.Now || startdatetime == null)
                return true;          
                return false;            
        }

        public bool AfterCurrentDate(string endDate)
        {
            var enddatetime = endDate.ToDateTimeMonthYear();
            if (enddatetime > DateTime.Now || enddatetime == null)
                return true;
            return false;
        }

        //public bool WorkStatusIsPast(int? statuscode)
        //{
        //    if (statuscode == 1)
        //        return true;
        //    return false;
        //}

    }
}
