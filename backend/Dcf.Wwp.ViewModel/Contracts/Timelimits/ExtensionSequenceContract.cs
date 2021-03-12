using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts.Timelimits
{
    public class ExtensionSequenceContract
    {
        public Int32 SequenceId { get; set; }

        public Int32 TimelimitTypeId { get; set; }

        public List<ExtensionContract> Extensions { get; } = new List<ExtensionContract>();

        public static ExtensionSequenceContract Create(Int32 timelimitTypeId,Int32 sequenceId, IEnumerable<ITimeLimitExtension> extensions)
        {
            var extSequenceContract = new ExtensionSequenceContract
            {
                SequenceId = sequenceId,
                TimelimitTypeId = timelimitTypeId
            };
            extSequenceContract.Extensions.AddRange( extensions.Select( ExtensionContract.Create ));
            

            return extSequenceContract;
        }
    }
}