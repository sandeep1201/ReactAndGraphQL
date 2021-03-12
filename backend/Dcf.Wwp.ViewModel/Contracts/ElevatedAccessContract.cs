using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ElevatedAccessContract
    {
        public int?      Id                     { get; set; }
        public int?      WorkerId               { get; set; }
        public int?      ParticipantId          { get; set; }
        public DateTime  AccessCreateDate       { get; set; }
        public int?      ElevatedAccessReasonId { get; set; }
        public string    Details                { get; set; }
        public bool?     IsDeleted              { get; set; }
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }
        public byte[]    RowVersion             { get; set; }
        public decimal?  PinNumber              { get; set; }
    }
}
