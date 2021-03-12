using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class HistoryValueContract
    {
        public int Id { get; set; }

        public object Value { get; set; }

        public string DisplayValue { get; set; }

        public string ChangeType { get; set; }

        public bool IsDeleted { get; set; }

        public string DeleteReason { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public byte[] RowVersion { get; set; }
    }
}