using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class GoalType : BaseEntity
    {
        #region Properties

        public string   Name              { get; set; }
        public int      EnrolledProgramId { get; set; }
        public int      SortOrder         { get; set; }
        public bool     IsDeleted         { get; set; }
        public string   ModifiedBy        { get; set; }
        public DateTime ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram EnrolledProgram { get; set; }

        #endregion
    }
}
