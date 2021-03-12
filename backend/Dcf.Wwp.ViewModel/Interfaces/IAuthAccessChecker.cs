using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Model;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IAuthAccessChecker
    {
        #region Properties

        

        #endregion

        #region Methods

        bool HasAccess(string supervisorId, List<MostRecentProgram> programs);

        #endregion
    }
}
