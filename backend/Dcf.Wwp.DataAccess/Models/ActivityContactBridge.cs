using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActivityContactBridge : BaseEntity
    {
        #region Properties

        public int?      ActivityId   { get; set; }
        public int?      ContactId    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Nav Properties

        public virtual Activity Activity { get; set; }
        public virtual Contact  Contact  { get; set; }

        #endregion

        #region Clone

        public ActivityContactBridge Clone()
        {
            var a = new ActivityContactBridge
                    {
                        Id           = Id,
                        IsDeleted    = IsDeleted,
                        ModifiedBy   = ModifiedBy,
                        ModifiedDate = ModifiedDate,
                        RowVersion   = RowVersion,
                        ActivityId   = ActivityId,
                        ContactId    = ContactId
                    };

            return a;
        }

        #endregion
    }
}
