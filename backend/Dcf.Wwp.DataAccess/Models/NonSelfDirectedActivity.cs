using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class NonSelfDirectedActivity : BaseEntity
    {
        #region Properties

        public int      ActivityId    { get; set; }
        public string   BusinessName  { get; set; }
        public int?     CityId        { get; set; }
        public decimal? PhoneNumber   { get; set; }
        public bool     IsDeleted     { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }
        public string   StreetAddress { get; set; }
        public string   ZipAddress    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Activity Activity { get; set; }
        public virtual City     City     { get; set; }

        #endregion

        #region Clone

        public NonSelfDirectedActivity Clone()
        {
            var a = new NonSelfDirectedActivity
                    {
                        Id            = Id,
                        IsDeleted     = IsDeleted,
                        ModifiedBy    = ModifiedBy,
                        ModifiedDate  = ModifiedDate,
                        RowVersion    = RowVersion,
                        ActivityId    = ActivityId,
                        BusinessName  = BusinessName,
                        CityId        = CityId,
                        PhoneNumber   = PhoneNumber,
                        StreetAddress = StreetAddress,
                        ZipAddress    = ZipAddress
                    };

            return a;
        }

        #endregion
    }
}
