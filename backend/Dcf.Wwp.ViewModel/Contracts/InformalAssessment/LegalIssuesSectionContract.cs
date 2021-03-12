using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class LegalIssuesSectionContract : BaseInformalAssessmentContract, IValidatableObject
    {
        private readonly IValidator _validator;

        public bool? IsConvictedOfCrime { get; set; }

        public List<ConvictionContract> Convictions { get; set; }

        public List<ConvictionContract> DeletedConvictions { get; set; }

        public bool? IsUnderCommunitySupervision { get; set; }

        public string CommunitySupervisonDetails { get; set; }

        // Supervision Contact.
        public int? SupervisionContactId { get; set; }

        public bool? IsPending { get; set; }

        public List<PendingContract> Pendings { get; set; }

        public bool? HasFamilyLegalIssues { get; set; }

        public string FamilyLegalIssueNotes { get; set; }

        // Child Support.
        public bool? HasChildSupport { get; set; }

        public string ChildSupportAmount { get; set; }

        public bool? IsAmountUnknown { get; set; }

        public bool?  HasBackChildSupport           { get; set; }
        public bool?  HasRestrainingOrders          { get; set; }
        public string RestrainingOrderNotes         { get; set; }
        public bool?  HasRestrainingOrderToPrevent   { get; set; }
        public string RestrainingOrderToPreventNotes { get; set; }

        public string ChildSupportDetails { get; set; }

        // Upcoming court dates?
        public bool? HasUpcomingCourtDates { get; set; }

        public List<CourtContract> CourtDates { get; set; }

        public ActionNeededContract ActionNeeded { get; set; }

        public string Notes { get; set; }

        #region Constructor

        public LegalIssuesSectionContract()
        {
            _validator = new LegalIssueSectionValidator();
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
