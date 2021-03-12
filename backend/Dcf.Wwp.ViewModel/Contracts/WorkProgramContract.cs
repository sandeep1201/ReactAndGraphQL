using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using Dcf.Wwp.Model.Interface;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;


namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WorkProgramContract : BaseRepeaterContract, IIsEmpty, IValidatableObject
    {
        private readonly IValidator _validator;

        public int? WorkStatus { get; set; }

        public string WorkStatusName { get; set; }

        public int? WorkProgram { get; set; }

        public string WorkProgramName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int? ContactId { get; set; }

        public LocationContract Location { get; set; }

        public string Details { get; set; }

        #region Constructor

        public WorkProgramContract()
        {
            _validator = new WorkProgramValidator();
        }

        #endregion

        #region IValidatableObject

        ///
        /// Determines whether the specified object is valid.
        ///
        ///The validation context.
        ///
        /// A collection that holds failed-validation information.
        ///
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = _validator.Validate(this).ToValidationResult();
            return results;
        }

        #endregion


        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return !WorkStatus.HasValue &&
                   !WorkProgram.HasValue &&
                   string.IsNullOrWhiteSpace(StartDate) &&
                   string.IsNullOrWhiteSpace(EndDate) &&
                   !ContactId.HasValue &&
                   string.IsNullOrWhiteSpace(Details) &&
                   (Location == null || Location.IsEmpty());
        }

        #endregion IIsEmpty


        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : WorkProgramContract
            where TModel : IInvolvedWorkProgram
        {
            Debug.Assert(contract.IsNew(), "WorkProgramContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values

            // As of now, we won't restore previous values.
            // TODO: Revisit this decision in the future.

            //if (model != null)
            //{
            //    if (contract.FirstName != model.FirstName ||
            //        contract.LastName != model.LastName)
            //        return false;

            //    // If we get here, we have a match.  Since it is, we need to adopt the model's
            //    // Id values.
            //    contract.Id = model.Id;
            //    return true;
            //}

            // This is a case where the model was null which shouldn't really happen.  If it does
            // though it's not going to be a match.
            return false;
        }
    }
}
