using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PlacementType : BaseEntity
    {
        #region Properties

        public string    Code            { get; set; }
        public string    Name            { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsSystemUseOnly { get; set; }
        public DateTime  EffectiveDate   { get; set; }
        public DateTime? EndDate         { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime  ModifiedDate    { get; set; }
        public bool      IsSanctionable  { get; set; }
        public decimal   PaymentAmount   { get; set; }
        public string    DB2Code         { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

        #region Clone

        #endregion
    }
}
