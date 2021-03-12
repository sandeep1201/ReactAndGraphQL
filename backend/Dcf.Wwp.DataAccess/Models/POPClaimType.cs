using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class POPClaimType : BaseEntity
    {
        #region Properties

        public string    Code               { get; set; }
        public string    Description        { get; set; }
        public decimal   MinimumHoursWorked { get; set; }
        public decimal   MinimumEarnings    { get; set; }
        public bool      IsSystemUseOnly    { get; set; }
        public DateTime  EffectiveDate      { get; set; }
        public DateTime? EndDate            { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

        #region Clone

        #endregion
    }
}
