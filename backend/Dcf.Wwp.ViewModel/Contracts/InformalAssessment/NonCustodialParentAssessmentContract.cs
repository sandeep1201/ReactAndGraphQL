using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class NonCustodialParentAssessmentContract : BaseInformalAssessmentContract
    {
        public bool?                               HasChildren                         { get; set; }
        public List<NonCustodialCaretakerContract> NonCustodialCaretakers              { get; set; }
        public List<NonCustodialCaretakerContract> DeletedNonCustodialCaretakers       { get; set; }
        public string                              ChildSupportPayment                 { get; set; }
        public bool?                               HasOwedChildSupport                 { get; set; }
        public bool?                               HasInterestInChildServices          { get; set; }
        public bool?                               IsInterestedInReferralServices      { get; set; }
        public string                              InterestedInReferralServicesDetails { get; set; }
        public ActionNeededContract                ActionNeeded                        { get; set; }
        public string                              Notes                               { get; set; }
        public int?                                ChildSupportContactId               { get; set; }
    }


    public class NonCustodialCaretakerContract : BaseRepeaterContract, IIsEmpty
    {
        public string                          FirstName                               { get; set; }
        public bool?                           IsFirstNameUnknown                      { get; set; }
        public string                          LastName                                { get; set; }
        public bool?                           IsLastNameUnknown                       { get; set; }
        public int?                            NonCustodialParentRelationshipId        { get; set; }
        public string                          NonCustodialParentRelationshipName      { get; set; }
        public string                          RelationshipDetails                     { get; set; }
        public int?                            ContactIntervalId                       { get; set; }
        public string                          ContactIntervalName                     { get; set; }
        public string                          ContactIntervalDetails                  { get; set; }
        public bool?                           IsRelationshipChangeRequested           { get; set; }
        public string                          RelationshipChangeRequestedDetails      { get; set; }
        public bool?                           IsInterestedInRelationshipReferral      { get; set; }
        public string                          InterestedInRelationshipReferralDetails { get; set; }
        public int?                            DeleteReasonId                          { get; set; }
        public List<NonCustodialChildContract> NonCustodialChilds                      { get; set; }
        public List<NonCustodialChildContract> DeletedNonCustodialChilds               { get; set; }


        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(FirstName)                          &&
                   (!IsFirstNameUnknown.HasValue || !IsFirstNameUnknown.Value)   &&
                   string.IsNullOrWhiteSpace(LastName)                           &&
                   (!IsLastNameUnknown.HasValue || !IsLastNameUnknown.Value)     &&
                   !NonCustodialParentRelationshipId.HasValue                    &&
                   string.IsNullOrWhiteSpace(RelationshipDetails)                &&
                   !ContactIntervalId.HasValue                                   &&
                   string.IsNullOrWhiteSpace(ContactIntervalDetails)             &&
                   !IsRelationshipChangeRequested.HasValue                       &&
                   string.IsNullOrWhiteSpace(RelationshipChangeRequestedDetails) &&
                   NonCustodialChilds.All(c => c.IsEmpty());
        }

        #endregion IIsEmpty


        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : NonCustodialCaretakerContract
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


    public class NonCustodialChildContract : BaseRepeaterContract, IIsEmpty
    {
        public string FirstName                          { get; set; }
        public string LastName                           { get; set; }
        public string DateOfBirth                        { get; set; }
        public bool?  HasChildSupportOrder               { get; set; }
        public bool?  HasNameOnChildBirthRecord          { get; set; }
        public string ChildSupportOrderDetails           { get; set; }
        public int?   ContactIntervalId                  { get; set; }
        public string ContactIntervalName                { get; set; }
        public string ContactIntervalDetails             { get; set; }
        public int?   HasOtherAdultsPolarLookupId        { get; set; }
        public string HasOtherAdultsPolarLookupName      { get; set; }
        public string OtherAdultsDetails                 { get; set; }
        public bool?  IsRelationshipChangeRequested      { get; set; }
        public string RelationshipChangeRequestedDetails { get; set; }
        public int?   IsNeedOfServicesPolarLookupId      { get; set; }
        public string IsNeedOfServicesPolarLookupName    { get; set; }
        public string NeedOfServicesDetails              { get; set; }
        public int?   DeleteReasonId                     { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(FirstName)                          &&
                   string.IsNullOrWhiteSpace(LastName)                           &&
                   string.IsNullOrWhiteSpace(DateOfBirth)                        &&
                   !HasChildSupportOrder.HasValue                                &&
                   string.IsNullOrWhiteSpace(ChildSupportOrderDetails)           &&
                   !HasNameOnChildBirthRecord.HasValue                           &&
                   !ContactIntervalId.HasValue                                   &&
                   string.IsNullOrWhiteSpace(ContactIntervalDetails)             &&
                   !HasOtherAdultsPolarLookupId.HasValue                         &&
                   string.IsNullOrWhiteSpace(OtherAdultsDetails)                 &&
                   !IsRelationshipChangeRequested.HasValue                       &&
                   string.IsNullOrWhiteSpace(RelationshipChangeRequestedDetails) &&
                   !IsNeedOfServicesPolarLookupId.HasValue                       &&
                   string.IsNullOrWhiteSpace(NeedOfServicesDetails);
        }

        #endregion IIsEmpty
    }
}
