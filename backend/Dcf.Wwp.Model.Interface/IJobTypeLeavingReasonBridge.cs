using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IJobTypeLeavingReasonBridge : ICommonModelFinal, ICloneable
    {
        int  Id              { get; set; }
        int  JobTypeId       { get; set; }
        int  LeavingReasonId { get; set; }
        bool IsDeleted       { get; set; }

        IJobType       JobType       { get; set; }
        ILeavingReason LeavingReason { get; set; }
    }
}
