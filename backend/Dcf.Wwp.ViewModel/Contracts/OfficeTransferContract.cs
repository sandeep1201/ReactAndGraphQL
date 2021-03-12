using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class OfficeTransferContract
    {
        public string NewFepId { get; set; }

        public bool? FepOutOfSync { get; set; }
    }
}
