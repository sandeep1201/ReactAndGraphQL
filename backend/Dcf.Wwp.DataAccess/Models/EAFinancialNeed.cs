using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAFinancialNeed : BaseEntity
    {
        #region Properties

        public int      RequestId           { get; set; }
        public int?     FinancialNeedTypeId { get; set; }
        public decimal? Amount              { get; set; }
        public bool     IsDeleted           { get; set; }
        public string   ModifiedBy          { get; set; }
        public DateTime ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest           EaRequest           { get; set; }
        public virtual EAFinancialNeedType EaFinancialNeedType { get; set; }

        #endregion
    }
}
