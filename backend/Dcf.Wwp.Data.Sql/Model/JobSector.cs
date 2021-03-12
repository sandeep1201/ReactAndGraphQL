using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobSector
    {
        #region Properties

        public string    Name         { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<OtherJobInformation> OtherJobInformations { get; set; }
        [JsonIgnore]
        public virtual ICollection<EmployerOfRecordInformation> EmployerOfRecordInformations { get; set; }

        #endregion
    }
}
