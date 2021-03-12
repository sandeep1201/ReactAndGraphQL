using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitState
    {
        #region Properties

        public string    Code         { get; set; }
        public string    Name         { get; set; }
        public int?      CountryId    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Country Country { get; set; }

        #endregion
    }
}
