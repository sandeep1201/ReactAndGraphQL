using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IJobType : ICommonModel, ICloneable
    {
        String                                   Name                        { get; set; }
        Boolean?                                 IsRequired                  { get; set; }
        Int32?                                   SortOrder                   { get; set; }
        bool                                     IsDeleted                   { get; set; }
        bool?                                    IsUsedForEmploymentOfRecord { get; set; }
        ICollection<IEmploymentInformation>      EmploymentInformations      { get; set; }
        ICollection<IJobTypeLeavingReasonBridge> JobTypeLeavingReasonBridges { get; set; }
    }
}
