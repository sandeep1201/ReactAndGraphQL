using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityContactBridge
    {
        #region Properties

        public int? ActivityId { get; set; }

        public int? ContactId { get; set; }

        //public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Nav Properties

        public virtual Activity Activity { get; set; }
        public virtual Contact  Contact  { get; set; }

        #endregion
    }
}
