﻿using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContactInterval
    {
        #region Properties

        public string    Name         { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<NonCustodialCaretaker> NonCustodialCaretakers { get; set; }
        public virtual ICollection<NonCustodialChild> NonCustodialChilds { get; set; }

        #endregion
    }
}