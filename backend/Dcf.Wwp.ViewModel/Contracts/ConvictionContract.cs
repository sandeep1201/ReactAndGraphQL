using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Extensions;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ConvictionContract : BaseContract, IValidatableObject, IIsEmpty
    {
        private readonly IValidator _validator;

        public int? Type { get; set; }

        public string TypeName { get; set; }

        public string Date { get; set; }

        public bool? IsDateUnknown { get; set; }

        public string Details { get; set; }

        public int? DeleteReasonId { get; set; }


        #region Constructor
        ///
        /// Initializes a new instance of the  class.
        ///
        public ConvictionContract()
        {
            _validator = new ConvictionValidator();
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
            return Type == null &&
                string.IsNullOrWhiteSpace(TypeName) &&
                string.IsNullOrWhiteSpace(Date) &&
                IsDateUnknown == null &&
                string.IsNullOrWhiteSpace(Details) &&
                DeleteReasonId == null;
        }

        #endregion IIsEmpty


        /// <summary>
        /// 
        /// Notes: 
        /// By passing every contract we map it to our model if select data matches.
        /// 
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="contract"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : ConvictionContract
            where TModel : IConviction
        {
            Debug.Assert(contract.IsNew(), "ConvictionContract is expected to be new, but it's not.");

            if (contract.Id != 0)
            {
                if (contract.Id == model.Id)
                    return true;


                return false;
            }


            Debug.WriteLine(model.ConvictionTypeID);
            Debug.WriteLine(model.DateConvicted.ToStringMonthYear());
            Debug.WriteLine(model.IsUnknown);
            Debug.WriteLine(model.Details);

            if (model != null)
            {
                if (contract.Type != model.ConvictionTypeID ||
                    //contract.TypeName != model.ConvictionType?.Name ||
                    contract.Date != model.DateConvicted.ToStringMonthYear() ||
                    contract.IsDateUnknown != model.IsUnknown ||
                    contract.Details != model.Details)
                    return false;

                contract.Id = model.Id;

                return true;
            }

            return true;
        }

    }
}
