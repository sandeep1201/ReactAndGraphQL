// ReSharper disable CollectionNeverQueried.Global

using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class BatchDetailsReportContract
    {
        public decimal?           CaseNumber   { get; set; }
        public List<BatchDetails> BatchDetails { get; set; }
    }

    public class BatchDetails
    {
        public int                Index                  { get; set; }
        public string             WeeklyBatchDate        { get; set; }
        public string             PreviousNPHours        { get; set; }
        public List<ActionsTaken> ActionsTaken           { get; set; }
        public string             CurrentNPHours         { get; set; }
        public string             Calculation            { get; set; }
        public string             OpOrAux                { get; set; }
        public bool               IncludesUnAppliedHours { get; set; }
    }

    public class ActionsTaken
    {
        public string   ActionTaken       { get; set; }
        public DateTime ParticipationDate { get; set; }
        public string   Code      { get; set; }
    }
}
