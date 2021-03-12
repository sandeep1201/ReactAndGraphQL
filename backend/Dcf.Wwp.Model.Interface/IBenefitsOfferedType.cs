using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBenefitsOfferedType : ICommonDelModel
    {
        String Name { get; set; }
        Boolean? DisablesOthersFlag { get; set; }
        Int32? SortOrder { get; set; }

        ICollection<IEmploymentInformationBenefitsOfferedTypeBridge> EmploymentInformationBenefitsOfferedTypeBridges { get; set; }
    }
}