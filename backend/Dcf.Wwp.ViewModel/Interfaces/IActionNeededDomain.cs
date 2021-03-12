using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IActionNeededDomain
    {
        #region Properties

        #endregion

        #region Methods

        ActionNeededContract GetActionNeededContract(Participant participant, string page);

        #endregion
    }
}
