using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEmploymentStatusTypeRepository
    {
        IEnumerable<IEmploymentStatusType> EmploymentStatusTypes();
        IEnumerable<IEmploymentStatusType> AllEmploymentStatusTypes();
        IEmploymentStatusType EmploymentStatusTypeByName(string name);
    }
}
