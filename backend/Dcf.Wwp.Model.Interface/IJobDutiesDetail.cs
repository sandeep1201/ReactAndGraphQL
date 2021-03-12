using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IJobDutiesDetail:ICommonModel,ICloneable
    {
        String Details { get; set; }
        Boolean IsDeleted { get; set; }
        Int32? SortOrder { get; set; }
        ICollection<IEmploymentInformationJobDutiesDetailsBridge> EmploymentInformationJobDutiesDetailsBridges { get; set; }
    }
}