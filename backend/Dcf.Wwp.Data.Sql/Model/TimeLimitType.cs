using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitType
    {
        #region Properties

        public string    Name         { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ICollection<ApprovalReason> ApprovalReasons { get; set; }

        [JsonIgnore]
        public virtual ICollection<DenialReason> DenialReasons { get; set; }

        public virtual ICollection<TimeLimitExtension> TimeLimitExtensions { get; set; }
        public virtual ICollection<TimeLimit>          TimeLimits          { get; set; }

        #endregion
    }
}
