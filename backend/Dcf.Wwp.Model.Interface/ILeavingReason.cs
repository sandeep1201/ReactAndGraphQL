using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ILeavingReason : ICommonModel, ICloneable
    {
        String                                   Name                        { get; set; }
        Int32?                                   SortOrder                   { get; set; }
        Boolean                                  IsDeleted                   { get; set; }
        ICollection<IEmploymentInformation>      EmploymentInformations      { get; set; }
        ICollection<IJobTypeLeavingReasonBridge> JobTypeLeavingReasonBridges { get; set; }
    }
}
