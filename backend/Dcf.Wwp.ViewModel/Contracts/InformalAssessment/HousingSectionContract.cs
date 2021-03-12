using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Contracts.Cww;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class HousingSectionContract : BaseInformalAssessmentContract, IValidatableObject
    {
        private readonly IValidator _validator;

        public int? HousingSituationId { get; set; }

        public string HousingSituationName { get; set; }

        public string CurrentHousingBeginDate { get; set; }

        public string CurrentHousingEndDate { get; set; }

        public bool? HasCurrentEvictionRisk { get; set; }

        public string CurrentHousingDetails { get; set; }

        public string CurrentMonthlyAmount { get; set; }

        public bool? IsCurrentAmountUnknown { get; set; }

        public bool? HasBeenEvicted { get; set; }

        public bool IsCurrentMovingToHistory { get; set; }

        public bool? HasUtilityDisconnectionRisk { get; set; }

        public string UtilityDisconnectionRiskNotes { get; set; }

        public bool? HasDifficultyWorking { get; set; }

        public string DifficultyWorkingNotes { get; set; }

        public ActionNeededContract ActionNeeded { get; set; }

        public List<HousingHistoryContract> Histories { get; set; }

        public string HousingNotes { get; set; }

        public List<CwwHousing> CwwHousing { get; set; }

        #region Constructor

        public HousingSectionContract()
        {
            _validator = new HousingSectionValidator();
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
