using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Api.Library.Contracts.Cww;


namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class NonCustodialParentReferralAssessmentContract : BaseInformalAssessmentContract
    {
        public int? HasChildrenId { get; set; }

        public string HasChildrenName { get; set; }

        public List<NonCustodialReferralParentContract> Parents { get; set; }

        public List<NonCustodialReferralParentContract> DeletedParents { get; set; }

        public string Notes { get; set; }

        public List<Child> CwwChildren { get; set; }
    }

    public class NonCustodialReferralParentContract : BaseRepeaterContract, IIsEmpty
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? IsAvailableOrWorking { get; set; }

        public string AvailableOrWorkingDetails { get; set; }

        public bool? IsInterestedInWorkProgram { get; set; }

        public string InterestedInWorkProgramDetails { get; set; }

        public bool? IsContactKnownWithParent { get; set; }

        public int? ContactId { get; set; }

        public int? DeleteReasonId { get; set; }

        public List<NonCustodialReferralChildContract> Children { get; set; }

        public List<NonCustodialReferralChildContract> DeletedChildren { get; set; }


        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(FirstName)                      &&
                   string.IsNullOrWhiteSpace(LastName)                       &&
                   !IsAvailableOrWorking.HasValue                            &&
                   string.IsNullOrWhiteSpace(AvailableOrWorkingDetails)      &&
                   !IsInterestedInWorkProgram.HasValue                       &&
                   string.IsNullOrWhiteSpace(AvailableOrWorkingDetails)      &&
                   !IsContactKnownWithParent.HasValue                        &&
                   !ContactId.HasValue                                       &&
                   string.IsNullOrWhiteSpace(InterestedInWorkProgramDetails) &&
                   Children.All(c => c.IsEmpty());
        }

        #endregion IIsEmpty


        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : NonCustodialReferralChildContract
            where TModel : INonCustodialCaretaker
        {
            Debug.Assert(contract.IsNew(), "NonCustodialCaretakerContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values

            if (model != null)
            {
                if (contract.FirstName != model.FirstName ||
                    contract.LastName  != model.LastName)
                    return false;

                // TODO: Handle Unknown??

                // If we get here, we have a match.  Since it is, we need to adopt the model's
                // Id values.
                contract.Id = model.Id;
                return true;
            }

            // This is a case where the model was null which shouldn't really happen.  If it does
            // though it's not going to be a match.
            return false;
        }
    }


    public class NonCustodialReferralChildContract : BaseRepeaterContract, IIsEmpty
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? HasChildSupportOrder { get; set; }

        public string ChildSupportOrderDetails { get; set; }

        public int? ContactIntervalId { get; set; }

        public string ContactIntervalName { get; set; }

        public string ContactIntervalDetails { get; set; }

        public int? DeleteReasonId { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            // NOTE: We are including ID in this because if an item has an ID
            // it means it has been saved to the database and there is not
            // actually empty.  It must be deleted with a delete reason.
            return (Id == 0)                                           &&
                   string.IsNullOrWhiteSpace(FirstName)                &&
                   string.IsNullOrWhiteSpace(LastName)                 &&
                   !HasChildSupportOrder.HasValue                      &&
                   string.IsNullOrWhiteSpace(ChildSupportOrderDetails) &&
                   string.IsNullOrWhiteSpace(ContactIntervalDetails)   &&
                   !ContactIntervalId.HasValue;
        }

        #endregion IIsEmpty
    }
}
