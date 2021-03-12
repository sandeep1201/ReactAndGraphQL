using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class DocumentContract
    {
        public int Id                { get; set; }
        public int EmployabilityPlanId { get; set; }
        public DateTime UploadedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
