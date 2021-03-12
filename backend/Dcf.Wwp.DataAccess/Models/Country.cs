using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Country : BaseEntity
    {
        #region Properties

        public string    Name          { get; set; }
        public string    Code          { get; set; }
        public bool      IsNonStandard { get; set; }
        public int?      SortOrder     { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<City>  Cities { get; set; } = new List<City>();
        public virtual ICollection<State> States { get; set; } = new List<State>();

        #endregion
    }
}
