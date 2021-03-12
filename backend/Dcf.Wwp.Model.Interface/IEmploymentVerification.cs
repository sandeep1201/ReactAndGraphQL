using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmploymentVerification : ICommonModelFinal
    {
        #region Properties

        int      EmploymentInformationId    { get; set; }
        bool     IsVerified                 { get; set; }
        DateTime CreatedDate                { get; set; }
        int?     NumberOfDaysAtVerification { get; set; }

        #endregion

        #region Navigation Props

        IEmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
