﻿using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActionPriority : BaseEntity
    {
        #region Properties

        public string    Name         { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

        #region Clone

        #endregion
    }
}