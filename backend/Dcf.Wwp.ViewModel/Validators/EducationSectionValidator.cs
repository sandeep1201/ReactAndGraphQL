using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using FluentValidation;



namespace Dcf.Wwp.Api.Library.Validators
{
  
    public class EducationSectionValidator: AbstractValidator<EducationHistorySectionContract>
    {
        public EducationSectionValidator()
        {
            ////Rule for School Name should not be more than 120 characters
            //RuleFor(x => x.SchoolName).Length(0, 120).WithMessage("Please enter not more than 120 characters");
            //Rule for Notes should not be more than 1000 characters
            RuleFor(x => x.Notes).Length(0, 1000).WithMessage("Please enter not more than 120 characters");
            //Rule for Last year Attended          
            //RuleFor(x => x.LastYearAttended).Cascade(CascadeMode.StopOnFirstFailure)
            //    .Must(lastYearAttended => lastYearAttended > 1000 && lastYearAttended < 10000 || lastYearAttended == null)
            //    .WithMessage("Last year attended should be a number and value should be 4 digits.")
            //    .Must(lastYearAttended => lastYearAttended <= ToIntYear(DateTime.Now) || lastYearAttended == null)
            //    .WithMessage("Last year attended should be past or present year");
            ////Rule for Last year Awarded          
            //RuleFor(x => x.CertificateYearAwarded).Cascade(CascadeMode.StopOnFirstFailure)
            //    .Must(certificateYearAwarded => certificateYearAwarded > 1000 && certificateYearAwarded < 10000 || certificateYearAwarded == null)
            //    .WithMessage("Last year Awarded should be a number and value should be 4 digits.")
            //    .Must(certificateYearAwarded => certificateYearAwarded <= ToIntYear(DateTime.Now) || certificateYearAwarded == null)           
            //    .WithMessage("Last year Awarded should be past or present year");
        }

        private int? ToIntYear(DateTime? now)
        {
            if (now == null)
                return null;

            var year = now?.Year;

            var datestring = year;

            return datestring;
        }

        //private bool BeFourDigitNumber(int? x)
        //{
        //    int i = 0;
        //    return int.TryParse(x, out 7);
        //}



    }
}
