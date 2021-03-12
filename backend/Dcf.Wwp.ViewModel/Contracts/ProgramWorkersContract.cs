using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ProgramWorkersContract
    {
        #region Properties

        public string ProgramName { get; set; }    
        public string ProgramCd { get; set; }        
        public List<WorkerContract> AgencyWorkers { get; set; }

        #endregion

        #region Constructors

        public ProgramWorkersContract()
        {
            // Always initialize the list of agency workers.
            AgencyWorkers = new List<WorkerContract>();
        }

        #endregion
    }
}
