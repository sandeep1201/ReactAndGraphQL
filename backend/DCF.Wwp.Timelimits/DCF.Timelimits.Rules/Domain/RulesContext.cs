using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Core.Domain;
using DCF.Core.Logging;

namespace DCF.Timelimits.Rules.Domain
{
    public class TimelimitParticipantContext
    {
        public Participant Participant { get; set; }
        public Timeline Timeline { get; set; }

    }
}