using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmploymentInformationBenefitsOfferedTypeBridge : ICommonDelModel
    {
        Int32? EmploymentInformationId { get; set; }
        Int32? BenefitsOfferedTypeId { get; set; }
        Int32? SortOrder { get; set; }

        IBenefitsOfferedType BenefitsOfferedType { get; set; }
        IEmploymentInformation EmploymentInformation { get; set; }
    }
}