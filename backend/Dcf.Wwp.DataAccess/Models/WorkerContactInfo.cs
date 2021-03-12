using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class WorkerContactInfo : BaseEntity
    {
        #region Navigation Properties

        public virtual Worker Worker { get; set; }

        #endregion

        #region Properties

        public int       WorkerId     { get; set; }
        public decimal   PhoneNumber  { get; set; }
        public string    Email        { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion
    }
}
