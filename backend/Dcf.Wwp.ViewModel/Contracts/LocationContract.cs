using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using Dcf.Wwp.Model.Interface;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class LocationContract : IGoogleLocation, IIsEmpty, IValidatableObject
    {
        private readonly IValidator _validator;

        public string Description    { get; set; }
        public string Longitude      { get; set; }
        public string Latitude       { get; set; }
        public string City           { get; set; }
        public string State          { get; set; }
        public string Country        { get; set; }
        public string FullAddress    { get; set; }
        public string ZipAddress     { get; set; }
        public string GooglePlaceId  { get; set; }
        public string AddressPlaceId { get; set; }
        public string AptUnit        { get; set; }

        #region Constructor

        ///
        /// Initializes a new instance of the  class.
        ///
        public LocationContract()
        {
            _validator = new LocationValidator();
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
            return string.IsNullOrWhiteSpace(Description) &&
                   string.IsNullOrWhiteSpace(City)        &&
                   string.IsNullOrWhiteSpace(Country)     &&
                   //string.IsNullOrWhiteSpace(FullAddress) &&
                   string.IsNullOrWhiteSpace(GooglePlaceId) &&
                   //string.IsNullOrWhiteSpace(Latitude) &&
                   //string.IsNullOrWhiteSpace(Longitude) &&
                   string.IsNullOrWhiteSpace(State);
        }

        #endregion IIsEmpty
    }
}
