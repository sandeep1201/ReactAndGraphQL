using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IEpEmploymentsDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<EpEmploymentContract> GetEmploymentsForEp(string                    pin,                 int    epId, DateTime epBeginDate, string programCd);
        List<EpEmploymentContract> UpsertEpEmployment(List<EpEmploymentContract> employmentsContract, string pin,  int      epId);

        #endregion
    }
}
