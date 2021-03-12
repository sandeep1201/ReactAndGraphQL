using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class CountyAndTribe : BaseEntity
    {
        #region Properties

        public short?   CountyNumber   { get; set; }
        public string   CountyName     { get; set; }
        public bool     IsCounty       { get; set; }
        public string   AgencyName     { get; set; }
        public short?   LocationNumber { get; set; }
        public bool     IsDeleted      { get; set; }
        public string   ModifiedBy     { get; set; }
        public DateTime ModifiedDate   { get; set; }
        public int?     SortOrder      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Office> Offices { get; set; } = new List<Office>();

        #endregion
    }
}
