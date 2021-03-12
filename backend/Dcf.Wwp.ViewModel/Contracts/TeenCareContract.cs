using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using Dcf.Wwp.Model.Interface;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class TeenCareContract : BaseContract, IValidatableObject, IIsEmpty
    {
        private readonly IValidator _validator;

        public int ChildId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string Details { get; set; }

        public int? DeleteReasonId { get; set; }
        

        #region Constructor
        ///
        /// Initializes a new instance of the  class.
        ///
        public TeenCareContract()
        {
            _validator = new TeenCareValidator();
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
            return string.IsNullOrWhiteSpace(FirstName) &&
                string.IsNullOrWhiteSpace(LastName) &&
                string.IsNullOrWhiteSpace(Details) &&
                string.IsNullOrWhiteSpace(DateOfBirth);
        }

        #endregion

        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : TeenCareContract
            where TModel : IChildYouthSectionChild
        {
            Debug.Assert(contract.IsNew(), "TeenCareContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values
            // already in the database is a bit trickier in this case as we have two
            // levels to deal with (IChildYouthSectionChild and IChild).

            // For the first check, we'll see if we have a ChildId value.  If so, that
            // trumps other checks.
            if (contract.ChildId != 0)
            {
                if (contract.ChildId == model.ChildId)
                {
                    // This is a case that isn't expected to happen IRL, but to be complete
                    // we will handle it.  We have a new ChildCareContract but it had a
                    // ChildId set, and since it matches the model.ChildId we will have the
                    // parent contract Id assume the model Id value.
                    contract.Id = model.Id;
                    return true;
                }

                // In this case, the contract has a child Id that is different so we'll just
                // continue using that value.  Don't think this could happen IRL.
                return false;
            }

            // Since there is no ChildId, we need to go down into the Child record of the
            // model and see if we have a match.
            if (model.Child != null)
            {
                if (contract.FirstName != model.Child.FirstName ||
                    contract.LastName != model.Child.LastName ||
                    contract.DateOfBirth != model.Child.DateOfBirth.ToStringMonthDayYear())
                    return false;

                // If we get here, we have a match.  Since it is, we need to adopt the model's
                // Id values.
                contract.Id = model.Id;
                // ReSharper disable once PossibleInvalidOperationException
                contract.ChildId = model.ChildId.Value;

                return true;
            }

            // OK, at this point we didn't have a Child in the database (model) record nor
            // did we have a Child ID from the contract so we are just looking for a match
            // on the ChildYouthSectionChild data.  And in this case where it is a teen, we
            // will use some hard coded values for the unused properties (CareArrangementId
            // and IsSpecialNeeds)
            if (null != model.CareArrangementId ||
                contract.Details != model.Details ||
                null != model.IsSpecialNeeds)
                return false;

            // Since we got here, there's a match.
            contract.Id = model.Id;

            return true;
        }

        #region Business Rules

        // NOTE: Be sure to keep this logic in sync with the user interface logic (Angular code).

        public int? AgeInYears
        {
            get
            {
                var dob = DateOfBirth.ToDateTimeMonthDayYear();

                if (dob.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - dob.Value.Year;
                    if (dob.Value > today.AddYears(-age))
                        age--;

                    return age;
                }

                return null;
            }
        }

        #endregion Business Rules
    }
}
