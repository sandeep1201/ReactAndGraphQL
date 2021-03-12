using Dcf.Wwp.Api.Library.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ISimulatedDateDomain
    {
        #region Properties
        #endregion

        #region Methods
        SimulatedDateContract GetSimulatedDate(int id);
        SimulatedDateContract UpsertSimulateDate(SimulatedDateContract simulatedDateContract);
        #endregion
    }
}
