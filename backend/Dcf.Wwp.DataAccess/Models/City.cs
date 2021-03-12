using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class City : BaseEntity
    {
        #region Properties

        public string    Name          { get; set; }
        public string    GooglePlaceId { get; set; }
        public int?      CountryId     { get; set; }
        public int?      StateId       { get; set; }
        public decimal?  Latitude      { get; set; }
        public decimal?  Longitude     { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Country Country { get; set; }
        public virtual State   State   { get; set; }

        #endregion
    }
}
