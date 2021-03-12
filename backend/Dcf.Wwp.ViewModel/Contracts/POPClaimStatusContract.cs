using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class POPClaimStatusContract
    {
        public int      Id                        { get;  set; }
        public int      POPClaimStatusTypeId      { get;  set; }
        public string   POPClaimStatusName        { get;  set; }
        public string   POPClaimStatusDisplayName { get;  set; }
        public DateTime POPClaimStatusDate        { get;  set; }
        public string   Details                   { get;  set; }
        public string   ModifiedBy                { get;  set; }
        public DateTime ModifiedDate              { get;  set; }
        public bool     IsDeleted                 {  get; set; }
    }
}
