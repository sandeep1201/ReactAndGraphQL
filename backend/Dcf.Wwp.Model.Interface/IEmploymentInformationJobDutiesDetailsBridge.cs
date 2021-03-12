using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmploymentInformationJobDutiesDetailsBridge:ICommonModel,ICloneable
    {
        Int32? EmploymentInformationId { get; set; }
        Int32? JobDutiesId { get; set; }
        Int32? SortOrder { get; set; }
        Boolean IsDeleted { get; set; }

        IJobDutiesDetail JobDutiesDetail { get; set; }
        IEmploymentInformation EmploymentInformation { get; set; }
    }
}