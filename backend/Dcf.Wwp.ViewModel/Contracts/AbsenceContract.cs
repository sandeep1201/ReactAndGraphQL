using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class AbsenceContract
    {
        public int Id { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public int? AbsenceReasonId { get; set; }

        public string Details { get; set; }

        public int SortOrder { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}