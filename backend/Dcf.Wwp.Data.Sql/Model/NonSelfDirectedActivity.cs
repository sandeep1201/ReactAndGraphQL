using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonSelfDirectedActivity
    {
        #region Properties

        public int      ActivityId    { get; set; }
        public string   BusinessName  { get; set; }
        public decimal? PhoneNumber   { get; set; }
        public string   StreetAddress { get; set; }
        public int?     CityId        { get; set; }
        public string   ZipAddress    { get; set; }
        public bool     IsDeleted     { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Activity Activity { get; set; }
        public virtual City     City     { get; set; }

        #endregion
    }
}
