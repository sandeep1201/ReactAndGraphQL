using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class EducationHistorySectionContract : BaseInformalAssessmentContract, IValidatableObject
    {
        private readonly IValidator _validator;

        public int? Diploma { get; set; }

        public string DiplomaName { get; set; }

        public LocationContract Location { get; set; }

        public string SchoolName { get; set; }

        public string Notes { get; set; }

        public int? CertificateYearAwarded { get; set; }

        public int? LastYearAttended { get; set; }

        public bool? IsCurrentlyEnrolled { get; set; }

        public int? LastGradeCompleted { get; set; }

        public string LastGradeCompletedName { get; set; }

        public bool? GedHsedStatus { get; set; }

        public bool? HasEverGoneToSchool { get; set; }

        public int? IssuingAuthorityCode { get; set; }

        public string IssuingAuthorityName { get; set; }

        public bool? HasEducationPlan { get; set; }

        public string EducationPlanDetails { get; set; }

        #region Constructor

        ///
        /// Initializes a new instance of the  class.
        ///
        public EducationHistorySectionContract()
        {
            _validator = new EducationSectionValidator();
        }
        #endregion

        #region
        /// Determines whether the specified object is valid.
        ///
        ///The validation context.
        ///
        /// A collection that holds failed-validation information.
        ///
        ///
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = _validator.Validate(this).ToValidationResult();
            return results;
        }

        #endregion
    }
}

