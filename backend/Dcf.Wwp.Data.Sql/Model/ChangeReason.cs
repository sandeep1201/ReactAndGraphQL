﻿using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChangeReason
    {
        #region Properties

        public string    Code         { get; set; }
        public string    Name         { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public bool?     IsRequired   { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<TimeLimit> TimeLimits { get; set; }

        #endregion
    }
}