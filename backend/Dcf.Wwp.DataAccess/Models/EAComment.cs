using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAComment : BaseEntity
    {
        #region Properties

        public int      RequestId    { get; set; }
        public bool     IsEdited     { get; set; }
        public string   Comment      { get; set; }
        public bool     IsDeleted    { get; set; }
        public DateTime CreatedDate  { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest                        EaRequest            { get; set; }
        public virtual ICollection<EACommentTypeBridge> EaCommentTypeBridges { get; set; } = new List<EACommentTypeBridge>();

        #endregion

        #region Clone

        #endregion
    }
}
