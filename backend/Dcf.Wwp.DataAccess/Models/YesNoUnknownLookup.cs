using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class YesNoUnknownLookup : BaseEntity
    {
        #region Properties

        public string Code { get; set; }
        public string Name { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<JRApplicationInfo> JrApplicationInfos { get; set; } = new List<JRApplicationInfo>();

        #endregion

        #region Clone

        #endregion
    }
}
