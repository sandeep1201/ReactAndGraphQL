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
    public class HousingHistoryContract:IValidatableObject
    {
        private readonly IValidator _validator;

        [DataMember(Name = "id")]
        public int Id { get; set; }
        
        [DataMember(Name = "historyType")]
        public int? HistoryType { get; set; }

        [DataMember(Name = "historyTypeName")]
        public string HistoryTypeName { get; set; }

        [DataMember(Name = "beginDate")]
        public string BeginDate { get; set; }

        [DataMember(Name = "endDate")]
        public string EndDate { get; set; }

        [DataMember(Name = "hasEvicted")]
        public bool? HasEvicted { get; set; }

        [DataMember(Name = "isAmountUnknown")]
        public bool? IsAmountUnknown { get; set; }

        [DataMember(Name = "monthlyAmount")]
        public string MonthlyAmount { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }

		public int SortOrder { get; set; }

        #region Constructor
        ///
        /// Initializes a new instance of the  class.
        ///
        public HousingHistoryContract()
        {
            _validator = new HousingHistoriesValidator();
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
