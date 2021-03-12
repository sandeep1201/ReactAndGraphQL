using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmployerOfRecordType : ICommonDelModel
    {
        #region Properties

        string Name      { get; set; }
        int?   SortOrder { get; set; }

        #endregion

        #region Navigation Properties

        ICollection<IEmploymentInformation> EmploymentInformations { get; set; }

        #endregion
    }
}
