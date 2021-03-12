using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class DocumentResponse
    {
        public string                MessageCode { get; set; }
        public List<DocumentRequest> Documents   { get; set; }
    }

    public class DocumentRequest
    {
        public int      EpId         { get; set; }
        public string   DocumentId   { get; set; }
        public DateTime UploadedDate { get; set; }
        public bool?    IsDeleted    { get; set; }
        public bool?    IsScanned    { get; set; }
        public bool?    IsSigned     { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
