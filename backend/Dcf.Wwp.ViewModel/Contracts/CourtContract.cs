using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts
{
	[DataContract]
    public class CourtContract:IValidatableObject
	{
	    private readonly IValidator _validator;

        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "location")]
		public string Location { get; set; }

		[DataMember(Name = "date")]
		public string Date { get; set; }

		[DataMember(Name = "isDateUnknown")]
		public bool? IsDateUnknown { get; set; }

		[DataMember(Name = "details")]
		public string Details { get; set; }

        #region Constructor
        ///
        /// Initializes a new instance of the  class.
        ///
        public CourtContract()
        {
            _validator = new CourtDatesValidator();
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

    }
}
