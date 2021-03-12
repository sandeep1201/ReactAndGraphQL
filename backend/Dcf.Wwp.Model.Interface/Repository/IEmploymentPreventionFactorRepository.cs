using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEmploymentPreventionTypeRepository
    {
        IEnumerable<IEmploymentPreventionType> EmploymentPreventionTypes();
        IEnumerable<IEmploymentPreventionType> AllEmploymentPreventionTypes();
        IEmploymentPreventionType EmploymentPreventionTypeById(int id);
    }
}