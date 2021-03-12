using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class PinComment : BaseEntity
    {
        #region Properties

        public int      ParticipantId { get; set; }
        public bool     IsEdited      { get; set; }
        public string   CommentText   { get; set; }
        public bool     IsDeleted     { get; set; }
        public DateTime CreatedDate    { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant             Participant { get; set; }
        public virtual ICollection<PCCTBridge> PCCTBridges { get; set; } = new List<PCCTBridge>();

        #endregion

        #region Clone

        #endregion
    }
}
