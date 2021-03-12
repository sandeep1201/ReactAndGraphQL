using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EARelationshipTypeBridge : BaseEntity
    {
        #region Properties

        public int      RelationshipTypeId { get; set; }
        public bool     IsOnlyForCR        { get; set; }
        public bool     IsDeleted          { get; set; }
        public string   ModifiedBy         { get; set; }
        public DateTime ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARelationshipType EaRelationshipType { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
