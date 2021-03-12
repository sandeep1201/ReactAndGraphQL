using System.Diagnostics;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PostSecondaryDegreeContract : BaseRepeaterContract, IIsEmpty
    {
        public string Name { get; set; }

        public int? Type { get; set; }

        public string TypeName { get; set; }

        public string College { get; set; }

        public int? YearAttained { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return !string.IsNullOrWhiteSpace(Name) &&
                   !Type.HasValue &&
                   !string.IsNullOrWhiteSpace(TypeName) &&
                   !string.IsNullOrWhiteSpace(College) &&
                   !YearAttained.HasValue;
        }

        #endregion IIsEmpty


        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : PostSecondaryDegreeContract
            where TModel : IPostSecondaryDegree
        {
            Debug.Assert(contract.IsNew(), "PostSecondaryDegreeContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values

            if (model != null)
            {
                if (contract.Name != model.Name ||
                    contract.Type != model.DegreeTypeId ||
                    contract.College != model.College ||
                    contract.YearAttained != model.YearAttained
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

