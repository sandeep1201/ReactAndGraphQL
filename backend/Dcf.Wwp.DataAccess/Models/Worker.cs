using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Worker : BaseEntity
    {
        #region Properties

        public string    WAMSId                 { get; set; }
        public string    MFUserId               { get; set; }
        public string    FirstName              { get; set; }
        public string    LastName               { get; set; }
        public string    MiddleInitial          { get; set; }
        public string    SuffixName             { get; set; }
        public string    Roles                  { get; set; }
        public string    WorkerActiveStatusCode { get; set; }
        public DateTime? LastLogin              { get; set; }
        public string    WIUId                  { get; set; }
        public int?      OrganizationId         { get; set; }
        public bool      IsDeleted              { get; set; }
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Organization                   Organization       { get; set; }
        public virtual ICollection<WorkerContactInfo> WorkerContactInfos { get; set; }
        public virtual ICollection<Transaction>       Transactions       { get; set; }

        #endregion
    }
}
