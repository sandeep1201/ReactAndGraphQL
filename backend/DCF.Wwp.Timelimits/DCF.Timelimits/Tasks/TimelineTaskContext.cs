using System;
using System.Collections.Generic;
using Dcf.Wwp.Data.Sql.Model;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Domain;
using MediatR;

namespace DCF.Timelimits.Tasks
{
    public class TimelineTaskContext<TK> : BatchTaskBase<TK> where TK: TimelineTaskResult
    {
        public Decimal PinNumber { get; set; }
        public ITimeline Timeline { get; set; } = new Timeline();
        public Participant Participant { get; set; } = new Participant();
        public List<AlienStatus> AlienStatus { get; set; } = new List<AlienStatus>();

        public override Boolean ReadyToProcess()
        {
            return true; // always process these

        }

        public override TK Result { get; set; }
    }

    public class TimelineTaskResult : BatchTaskResultBase
    {

    }
}