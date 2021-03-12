using System.Diagnostics;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PostSecondaryLicenseContract : BaseRepeaterContract, IIsEmpty
    {
        public int? LicenseType { get; set; }

        public string LicenseTypeName { get; set; }

        public string Name { get; set; }

        public int? PolarLookupId { get; set; }

        public string ValidInWi { get; set; }

        public string Issuer { get; set; }

        public string ExpiredDate { get; set; }

        public string AttainedDate { get; set; }

        public bool? DoesNotExpire { get; set; }

        public bool? IsInProgress { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return !string.IsNullOrWhiteSpace(Name) &&
                   !LicenseType.HasValue &&
                   !string.IsNullOrWhiteSpace(Issuer) &&
                   !string.IsNullOrWhiteSpace(ExpiredDate) &&
                   !string.IsNullOrWhiteSpace(AttainedDate) &&
                   !PolarLookupId.HasValue &&
                   !DoesNotExpire.HasValue &&
                   !IsInProgress.HasValue;
        }

        #endregion IIsEmpty


        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : PostSecondaryLicenseContract
            where TModel : IPostSecondaryLicense
        {
            Debug.Assert(contract.IsNew(), "PostSecondaryLicenseContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values

            if (model != null)
            {
                if (contract.Name != model.Name ||
                    contract.LicenseType != model.LicenseTypeId ||
                    contract.PolarLookupId != model.ValidInWIPolarLookupId ||
                    contract.Issuer != model.Issuer ||
                    contract.ExpiredDate != model.ExpiredDate.ToStringMonthDayYear() ||
                    contract.AttainedDate != model.AttainedDate.ToStringMonthDayYear() ||
                    contract.DoesNotExpire != model.DoesNotExpire ||
                    contract.IsInProgress != model.IsInProgress
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
