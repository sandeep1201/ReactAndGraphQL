using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PullDownDate : BaseEntity
    {
        #region Properties

        public int      BenefitYear      { get; set; }
        public int      BenefitMonth     { get; set; }
        public DateTime PullDownDates    { get; set; }
        public DateTime DelayedCycleDate { get; set; }
        public bool     IsDeleted        { get; set; }
        public string   ModifiedBy       { get; set; }
        public DateTime ModifiedDate     { get; set; }
        public short    NoOfDaysInPeriod { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
