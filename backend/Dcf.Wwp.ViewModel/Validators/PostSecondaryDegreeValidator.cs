using System;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class PostSecondaryDegreeValidator : AbstractValidator<PostSecondaryDegreeContract>
    {
        public PostSecondaryDegreeValidator()
        {
            RuleFor(x => x.Name).Length(0, 120).WithMessage("Please enter not more than 120 characters");
            RuleFor(x => x.College).Length(0, 120).WithMessage("Please enter not more than 120 characters");
            //Rule for YearAttained         
            RuleFor(x => x.YearAttained).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(yearattained => yearattained > 1000 && yearattained < 10000 || yearattained == null)
                .WithMessage("Month and Year Attained should be a number and value should be 4 digits.")
                .Must(lastYearAttended => lastYearAttended <= ToIntYear(DateTime.Now) || lastYearAttended == null)
                .WithMessage("Month and Year Attained attended should be past or present year");
        }

        private int? ToIntYear(DateTime? now)
        {
            if (now == null)
                return null;

            var year = now?.Year;

            var datestring = year;

            return datestring;
        }
    }
}
