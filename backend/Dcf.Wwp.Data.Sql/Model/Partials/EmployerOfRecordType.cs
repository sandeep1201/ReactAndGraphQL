using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployerOfRecordType : BaseEntity, IEmployerOfRecordType
    {
        #region Properties

        #endregion

        #region Navigation Properties

        ICollection<IEmploymentInformation> IEmployerOfRecordType.EmploymentInformations
        {
            get => EmploymentInformations.Cast<IEmploymentInformation>().ToList();

            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        #endregion
    }
}
