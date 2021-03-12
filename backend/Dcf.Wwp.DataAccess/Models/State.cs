using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class State : BaseEntity
    {
        #region Properties

        public string   Code          { get; set; }
        public string   Name          { get; set; }
        public int?     CountryId     { get; set; }
        public bool     IsNonStandard { get; set; }
        public bool     IsDeleted     { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Country           Country { get; set; }
        public virtual ICollection<City> Cities  { get; set; } = new List<City>();

        #endregion
    }
}
