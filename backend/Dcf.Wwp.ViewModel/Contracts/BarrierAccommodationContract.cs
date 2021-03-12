using System.Diagnostics;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class BarrierAccommodationContract : BaseContract, IIsEmpty
    {
        public int? AccommodationId { get; set; }

        public string AccommodationName { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public string Details { get; set; }

        public int? DeleteReasonId { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(BeginDate) &&
                   string.IsNullOrWhiteSpace(EndDate) &&
                   string.IsNullOrWhiteSpace(Details) &&
                   AccommodationId == null;
        }

        #endregion IIsEmpty

        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
           where TContract : BarrierAccommodationContract
           where TModel : IBarrierAccommodation
        {
            Debug.Assert(contract.IsNew(), "Barrier Accomodation is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values

            if (model != null)
            {
                if (contract.BeginDate != model.BeginDate.ToStringMonthDayYear() ||
                    contract.EndDate != model.EndDate.ToStringMonthDayYear() ||
                    contract.AccommodationId != model.AccommodationId ||
                    contract.Details != model.Details
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
