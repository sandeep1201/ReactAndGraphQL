using System.Diagnostics;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class FamilyMemberContract: BaseContract, IIsEmpty
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? RelationshipId { get; set; }

        public string RelationshipName { get; set; }

        public string Details { get; set; }

        public int? DeleteReasonId { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(FirstName) &&
                string.IsNullOrWhiteSpace(LastName) &&
                   string.IsNullOrWhiteSpace(Details) &&
                   RelationshipId == null;
        }

        #endregion IIsEmpty

        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : FamilyMemberContract
            where TModel : IFamilyMember
        {
            Debug.Assert(contract.IsNew(), "FamilyMemberContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values
           
            if (model != null)
            {
                if (contract.FirstName != model.FirstName ||
                    contract.LastName != model.LastName ||
                    //contract.Details != model.Details ||      // We will allow different details but it still be considered the same person.
                    contract.RelationshipId != model.RelationshipId 
                    )
                    return false;

                // If we get here, we have a match.  Since it is, we need to adopt the model's
                // Id values.
                contract.Id = model.Id;
                return true;
            }
            return true;
        }
    }
}
