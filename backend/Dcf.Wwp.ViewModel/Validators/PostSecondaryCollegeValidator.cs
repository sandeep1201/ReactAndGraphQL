using System;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class PostSecondaryCollegeValidator : AbstractValidator<PostSecondaryCollegeContract>
    {
        public PostSecondaryCollegeValidator()
        {
            //RuleFor(x => x.Name)
            //    .Length(0, 120)
            //    .WithMessage("Name cannot be more 120 characters");
            //RuleFor(x => x.Location)
            //    .NotNull()
            //    .WithMessage("Location has to be still selected from autocomplete lists and cannot be empty");
            ////Rule for Last year Attended          
            //RuleFor(x => x.LastYearAttended).Cascade(CascadeMode.StopOnFirstFailure)
            //    .Must(lastYearAttended => lastYearAttended > 1000 && lastYearAttended < 10000 || lastYearAttended == null)
            //    .WithMessage("Last year attended should be a number and value should be 4 digits.")
            //    .Must(lastYearAttended => lastYearAttended <= ToIntYear(DateTime.Now) || lastYearAttended == null)
            //    .WithMessage("Last year attended should be past or present year");
            //RuleFor(x => x.Semesters)
            //    .Must(semesters => semesters > 0 && semesters < 99 || semesters == null)
            //    .WithMessage("Semesters should be a number and value should be 0 and 99");
            //RuleFor(x => x.Credits)
            //    .Must(credits => credits > 0 && credits < 999 || credits == null)
            //    .WithMessage("Credits  should be a number and value should be 0 and 999");
            //RuleFor( x => x.Details)
            //     .Length(0, 380)
            //    .WithMessage("Name cannot be more than 380 characters");

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