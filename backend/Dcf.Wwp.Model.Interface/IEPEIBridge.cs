using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEPEIBridge : ICommonModelFinal
    {
        #region Properties

        int? EmployabilityPlanId     { get; set; }
        int? EmploymentInformationId { get; set; }

        #endregion

        #region Navigation Props

        IEmployabilityPlan     EmployabilityPlan     { get; set; }
        IEmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
