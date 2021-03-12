using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JRWorkPreferenceShiftBridge : BaseEntity
    {
        #region Properties

        public int      WorkPreferenceId { get; set; }
        public int      WorkShiftId      { get; set; }
        public bool     IsDeleted        { get; set; }
        public string   ModifiedBy       { get; set; }
        public DateTime ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JRWorkPreferences JrWorkPreferences { get; set; }
        public virtual JRWorkShift       JrWorkShift       { get; set; }

        #endregion
    }
}
