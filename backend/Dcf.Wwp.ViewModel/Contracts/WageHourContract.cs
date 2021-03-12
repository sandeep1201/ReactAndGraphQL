using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class WageHourContract : BaseInformalAssessmentContract, IValidatableObject
    {
        private readonly IValidator _validator;

        [DataMember(Name = "currentEffectiveDate")]
        public string CurrentEffectiveDate { get; set; }

        [DataMember(Name = "currentPayType")]
        public JobActionTypeContract WageHourAction { get; set; }

        [DataMember(Name = "currentPayTypeDetails")]
        public string CurrentPayTypeDetails { get; set; }

        [DataMember(Name = "currentAverageWeeklyHours")]
        public string CurrentAverageWeeklyHours { get; set; }

        [DataMember(Name = "currentPayRate")]
        public string CurrentPayRate { get; set; }

        [DataMember(Name = "currentPayRateIntervalId")]
        public int? CurrentPayRateIntervalId { get; set; }

        [DataMember(Name = "currentPayRateIntervalName")]
        public string CurrentPayRateIntervalName { get; set; }

        [DataMember(Name = "pastBeginPayRate")]
        public string PastBeginPayRate { get; set; }

        [DataMember(Name = "pastBeginPayRateIntervalId")]
        public int? PastBeginPayRateIntervalId { get; set; }

        [DataMember(Name = "pastBeginPayRateIntervalName")]
        public string PastBeginPayRateIntervalName { get; set; }

        [DataMember(Name = "pastEndPayRate")]
        public string PastEndPayRate { get; set; }

        [DataMember(Name = "pastEndPayRateIntervalId")]
        public int? PastEndPayRateIntervalId { get; set; }

        [DataMember(Name = "pastEndPayRateIntervalName")]
        public string PastEndRateIntervalName { get; set; }

        [DataMember(Name = "currentHourlySubsidyRate")]
        public string CurrentHourlySubsidyRate { get; set; }

        [DataMember(Name = "unchangedPastPayRateIndicator")]
        public bool? IsUnchangedPastPayRateIndicator { get; set; }

        [DataMember(Name = "wageHourHistories")]
        public List<WageHourHistoryContract> WageHourHistories { get; set; }

        [DataMember(Name = "computedCurrentWageRateUnit")]
        public string ComputedCurrentWageRateUnit { get; set; }

        [DataMember(Name = "computedCurrentWageRateValue")]
        public string ComputedCurrentWageRateValue { get; set; }

        [DataMember(Name = "computedPastEndWageRateUnit")]
        public string ComputedPastEndWageRateUnit { get; set; }

        [DataMember(Name = "computedPastEndWageRateValue")]
        public string ComputedPastEndWageRateValue { get; set; }

        [DataMember(Name = "computedDB2WageRateUnit")]
        public string ComputedDB2WageRateUnit { get; set; }

        [DataMember(Name = "computedDB2WageRateValue")]
        public string ComputedDB2WageRateValue { get; set; }

        [DataMember(Name = "workSiteContribution")]
        public string WorkSiteContribution { get; set; }

        #region Constructor

        ///
        /// Initializes a new instance of the  class.
        ///
        public WageHourContract()
        {
            _validator = new WagehourValidator();
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
