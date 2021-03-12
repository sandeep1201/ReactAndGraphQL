using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JRHistoryInfo : BaseEntity
    {
        #region Properties

        public int      JobReadinessId        { get; set; }
        public string   LastJobDetails        { get; set; }
        public string   AccomplishmentDetails { get; set; }
        public string   StrengthDetails       { get; set; }
        public string   AreasNeedImprove      { get; set; }
        public bool     IsDeleted             { get; set; }
        public string   ModifiedBy            { get; set; }
        public DateTime ModifiedDate          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobReadiness JobReadiness { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
