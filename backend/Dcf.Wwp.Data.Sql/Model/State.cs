using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class State
    {
        #region Properties

        public string    Name          { get; set; }
        public string    Code          { get; set; }
        public int?      CountryId     { get; set; }
        public bool      IsNonStandard { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual Country Country { get; set; }

        [JsonIgnore]
        public virtual ICollection<City> Cities { get; set; }

        #endregion
    }
}
