using System.Diagnostics;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ChildContract : BaseContract, IIsEmpty
    {
        public int    ChildId     { get; set; }
        public string FirstName   { get; set; }
        public string LastName    { get; set; }
        public string DateOfBirth { get; set; }
        public int?   GenderId    { get; set; }
        public string Gender      { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(FirstName) &&
                   string.IsNullOrWhiteSpace(LastName) &&
                   !GenderId.HasValue &&
                   string.IsNullOrWhiteSpace(DateOfBirth);
        }

        #endregion IIsEmpty

        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : ChildContract
            where TModel : IRequestForAssistanceChild
        {
            Debug.Assert(contract.IsNew(), "ChildContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values
            // already in the database is a bit trickier in this case as we have two
            // levels to deal with (IChildYouthSectionChild and IChild).

            // For the first check, we'll see if we have a ChildId value.  If so, that
            // trumps other checks.
            if (contract.Id != 0)
            {
                if (contract.ChildId == model.Child?.Id)
                {
                    // This is a case that isn't expected to happen IRL, but to be complete
                    // we will handle it.  We have a new ChildCareContract but it had a
                    // ChildId set, and since it matches the model.ChildId we will have the
                    // parent contract Id assume the model Id value.
                    contract.Id = model.Id;
                    return true;
                }

                // In this case, the contract has a child Id that is different so we'll just
                // continue using that value.  Don't think this could happen IRL.
                return false;
            }

            // Since there is no ChildId, we need to go down into the Child record of the
            // model and see if we have a match.
            if (model.Child != null)
            {
                if (contract.FirstName != model.Child.FirstName ||
                    contract.LastName != model.Child.LastName ||
                    contract.DateOfBirth != model.Child.DateOfBirth.ToStringMonthDayYear())
                    return false;

                // If we get here, we have a match.  Since it is, we need to adopt the model's
                // Id values.
                contract.Id = model.Id;
                contract.ChildId = model.Child?.Id ?? new int();

                return true;
            }

            // Since we got here, there's a match.
            contract.Id = model.Id;

            return true;
        }
    }
}
