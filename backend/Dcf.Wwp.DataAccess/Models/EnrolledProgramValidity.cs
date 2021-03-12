using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EnrolledProgramValidity : BaseEntity
    {
        #region Properties

        public int?     EnrolledProgramId       { get; set; }
        public int?     MaxDaysCanBackDate      { get; set; }
        public int?     MaxDaysInProgressStatus { get; set; }
        public int?     MaxDaysCanBackDatePS    { get; set; }
        public int      SortOrder               { get; set; }
        public bool     IsDeleted               { get; set; }
        public string   ModifiedBy              { get; set; }
        public DateTime ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram EnrolledProgram { get; set; }

        #endregion
    }
}
