using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IDrugScreeningDomain
    {
        #region Properties

        #endregion

        #region Methods

        DrugScreeningContract GetDrugScreeningForPin(decimal pin);

        DrugScreeningContract UpsertDrugScreening(DrugScreeningContract drugScreeningContract, string pin);

        #endregion
    }
}
