using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmploymentWorkHistoryBridge : ICommonModel
    {
        Int32? WorkHistorySectionId { get; set; }
        Int32? ActionNeededId { get; set; }
        Boolean? IsDeleted { get; set; }
        IActionNeeded ActionNeeded { get; set; }
        IWorkHistorySection WorkHistorySection { get; set; }
    }
}