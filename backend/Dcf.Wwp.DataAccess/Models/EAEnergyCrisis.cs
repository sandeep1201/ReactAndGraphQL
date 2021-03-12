using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAEnergyCrisis : BaseEntity
    {
        #region Properties

        public int      RequestId                { get; set; }
        public bool?    InNeedForUtilities       { get; set; }
        public string   DifficultyForUtilityBill { get; set; }
        public string   ExistingAppliedHelp      { get; set; }
        public bool?    HaveThreat               { get; set; }
        public bool     IsDeleted                { get; set; }
        public string   ModifiedBy               { get; set; }
        public DateTime ModifiedDate             { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest EaRequest { get; set; }

        #endregion
    }
}
