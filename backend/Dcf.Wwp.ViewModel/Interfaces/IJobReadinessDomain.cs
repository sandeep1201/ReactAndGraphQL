using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IJobReadinessDomain
    {
        #region Properties

        #endregion

        #region Methods

        JobReadinessContract GetJobReadinessForPin(decimal           pin);
        JobReadinessContract UpsertJobReadiness(JobReadinessContract jobReadinessContract, string pin, int id, bool hasSaveErrors);

        #endregion
    }
}
