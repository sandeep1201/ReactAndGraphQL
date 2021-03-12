using Dcf.Wwp.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmployabilityPlanStatusType: BaseEntity
    {
        #region Properties

        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        //#region Navigation Properties

        //public virtual ICollection<EnrolledProgramEPActivityTypeBridge> EnrolledProgramEPActivityTypeBridges { get; set; } = new List<EnrolledProgramEPActivityTypeBridge>();

        //#endregion
    }
}
