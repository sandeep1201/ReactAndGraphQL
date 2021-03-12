using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEmploymentPreventionTypeRepository
    {
        public IEnumerable<IEmploymentPreventionType> EmploymentPreventionTypes()
        {
            return from x in _db.EmploymentPreventionTypes where !x.IsDeleted orderby x.SortOrder select x;
        }

        public IEnumerable<IEmploymentPreventionType> AllEmploymentPreventionTypes()
        {
            return from x in _db.EmploymentPreventionTypes orderby x.SortOrder select x;
        }

        public IEmploymentPreventionType EmploymentPreventionTypeById(int id)
        {
            return (from x in _db.EmploymentPreventionTypes where x.Id == id select x).SingleOrDefault();
        }
    }
}