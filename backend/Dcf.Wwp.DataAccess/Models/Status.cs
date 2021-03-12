using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipationStatusType : BaseEntity
    {
        #region Properties

        public string    Name          { get; set; }
        public string    Code          { get; set; }
        public int       SortOrder     { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime  ModifiedDate  { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate       { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
