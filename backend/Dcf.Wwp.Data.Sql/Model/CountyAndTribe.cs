using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CountyAndTribe
    {
        #region Properties

        public short?    CountyNumber   { get; set; }
        public string    CountyName     { get; set; }
        public bool      IsCounty       { get; set; }
        public string    AgencyName     { get; set; }
        public short?    LocationNumber { get; set; }
        public int?      SortOrder      { get; set; }
        public bool      IsDeleted      { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ICollection<Office> Offices { get; set; }

        #endregion
    }
}
