using Dcf.Wwp.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.DataAccess.Models
{
    public class SimulatedDate : BaseEntity
    {
        #region
        public string WUID { get; set; }
        public DateTime StartTimeStamp { get; set; }
        public DateTime? EndTimeStamp { get; set; }
        public DateTime CDODate { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region  Navigation Properties
        #endregion

    }
}
