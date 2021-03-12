using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class AuxiliaryStatus : BaseEntity
    {
        #region Properties

        public int      AuxiliaryId           { get; set; }
        public int      AuxiliaryStatusTypeId { get; set; }
        public DateTime AuxiliaryStatusDate   { get; set; }
        public string   Details               { get; set; }
        public bool     IsDeleted             { get; set; }
        public string   ModifiedBy            { get; set; }
        public DateTime ModifiedDate          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Auxiliary           Auxiliary           { get; set; }
        public virtual AuxiliaryStatusType AuxiliaryStatusType { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
