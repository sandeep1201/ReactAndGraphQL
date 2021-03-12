﻿using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Element : BaseEntity
    {
        #region Properties

        public string   Name         { get; set; }
        public DateTime      EffectiveDate    { get; set; }
        public DateTime? EndDate { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Nav Props
        #endregion
    }
}
