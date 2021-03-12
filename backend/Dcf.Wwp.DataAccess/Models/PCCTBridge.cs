using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PCCTBridge : BaseEntity
    {
        #region Properties

        public int      PinCommentId  { get; set; }
        public int      CommentTypeId { get; set; }
        public bool     IsDeleted     { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual PinComment     PinComment     { get; set; }
        public virtual PinCommentType PinCommentType { get; set; }

        #endregion
    }
}
