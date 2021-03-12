using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAHouseHoldIncome : BaseEntity
    {
        #region Properties

        public int      RequestId          { get; set; }
        public string   IncomeType         { get; set; }
        public decimal? MonthlyIncome      { get; set; }
        public int?     VerificationTypeId { get; set; }
        public int?     GroupMember        { get; set; }
        public bool     IsDeleted          { get; set; }
        public string   ModifiedBy         { get; set; }
        public DateTime ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest          EaRequest          { get; set; }
        public virtual EAVerificationType EaVerificationType { get; set; }
        public virtual Participant        Participant        { get; set; }

        #endregion
    }
}
