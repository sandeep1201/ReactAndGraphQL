using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class DrugScreeningStatus : BaseEntity
    {
        #region Properties

        public int      DrugScreeningId           { get; set; }
        public int      DrugScreeningStatusTypeId { get; set; }
        public DateTime DrugScreeningStatusDate   { get; set; }
        public string   Details                   { get; set; }
        public bool     IsDeleted                 { get; set; }
        public string   ModifiedBy                { get; set; }
        public DateTime ModifiedDate              { get; set; }

        #endregion

        #region Navigation Properties

        public virtual DrugScreening           DrugScreening           { get; set; }
        public virtual DrugScreeningStatusType DrugScreeningStatusType { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
