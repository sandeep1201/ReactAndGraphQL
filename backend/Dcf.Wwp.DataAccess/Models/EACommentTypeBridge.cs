using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EACommentTypeBridge : BaseEntity
    {
        #region Properties

        public int      CommentId     { get; set; }
        public int      CommentTypeId { get; set; }
        public bool     IsDeleted     { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EAComment     EaComment     { get; set; }
        public virtual EACommentType EaCommentType { get; set; }

        #endregion
    }
}
