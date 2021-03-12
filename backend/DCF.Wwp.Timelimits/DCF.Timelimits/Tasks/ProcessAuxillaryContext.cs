using System;
using System.Collections.Generic;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace DCF.Timelimits.Tasks
{
    public class ProcessAuxillaryContext : TimelineTaskContext<ProcessAuxillaryResult>
    {
        public List<AuxiliaryPayment> AuxiliaryPayments { get; set; } = new List<AuxiliaryPayment>();
        public IChangeReason AuxChangeReason { get; set; }
    }


    public class ProcessAuxillaryResult : TimelineTaskResult
    {
        public List<AuxiliaryPayment> ProcessedPayments { get; set; } = new List<AuxiliaryPayment>();
    }
}