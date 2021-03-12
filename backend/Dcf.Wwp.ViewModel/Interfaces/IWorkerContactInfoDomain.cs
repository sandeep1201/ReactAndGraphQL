using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IWorkerContactInfoDomain
    {
        #region Properties

        #endregion

        #region Methods

        WorkerInfoContract GetContactInfo();

        WorkerInfoContract UpsertIContactInformation(WorkerInfoContract workerInfoContract);

        #endregion
    }
}
