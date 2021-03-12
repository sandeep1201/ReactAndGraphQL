﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class CourtDatesValidator : AbstractValidator<CourtContract>
    {
        public CourtDatesValidator()
        {
            //RuleFor(x => x.Date).Cascade(CascadeMode.StopOnFirstFailure)
            //      .Length(1, 8)
            //      .WithMessage("Conviction date should be a number and value should be 6 digits.")
            //      .Must(InBetweenOneAndTwelve)
            //      .WithMessage("Conviction date month should be between 01 and 12")
            //      .Must(PriorToCurrentDate)
            //      .WithMessage("Last year attended should be past or present year");
        }

        public bool PriorToCurrentDate(string startDate)
        {
            var startdatetime = startDate.ToDateTimeMonthYear();
            if (startdatetime < DateTime.Now || startdatetime == null)
                return true;

            return false;
        }

        public bool InBetweenOneAndTwelve(string startDate)
        {
            var startdatetime = startDate.GetMonthInt();
            if (startdatetime >= 1 && startdatetime <= 12 || startdatetime == null)
                return true;

            return false;
        }
    }
}
