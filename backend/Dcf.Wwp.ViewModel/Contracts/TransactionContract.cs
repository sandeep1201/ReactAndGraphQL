using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class TransactionContract
    {
        public int      Id                         { get;  set; }
        public int      ParticipantId              { get;  set; }
        public int?     WorkerId                   { get;  set; }
        public int      OfficeId                   { get;  set; }
        public int      TransactionTypeId          { get;  set; }
        public string   Description                { get;  set; }
        public DateTime EffectiveDate              { get;  set; }
        public DateTime CreatedDate                { get;  set; }
        public bool     IsDeleted                  { get;  set; }
        public string   ModifiedBy                 { get;  set; }
        public DateTime ModifiedDate               { get;  set; }
        public string   WorkerName                 { get;  set; }
        public int      AgencyId                   { get;  set; }
        public string   AgencyName                 { get ; set; }
        public string   CountyName                 { get;  set; }
        public string   TransactionTypeName        { get;  set; }
        public string   TransactionTypeCode        { get;  set; }
        public string   TransactionTypeDescription { get;  set; }
        public string   StatusCode                 { get;  set; }
    }
}
