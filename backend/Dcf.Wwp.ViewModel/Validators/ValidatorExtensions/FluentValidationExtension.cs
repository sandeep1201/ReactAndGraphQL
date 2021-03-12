using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;


namespace Dcf.Wwp.Api.Library.Validators.ValidatorExtensions
{
    public static  class FluentValidationExtension
    {
        /// <summary>
        /// Converts the Fluent Validation result to the type the both mvc and ef expect
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> ToValidationResult(this FluentValidation.Results.ValidationResult validationResult)
        {
            var results = validationResult.Errors.Select(item => new ValidationResult(item.ErrorMessage, new List<string> { item.PropertyName }));
            return results;
        }
    }
}
