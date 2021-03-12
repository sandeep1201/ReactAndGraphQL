using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IEnumerable<IEmploymentStatusType> EmploymentStatusTypes()
        {
            return from est in _db.EmploymentStatusTypes where !est.IsDeleted orderby est.SortOrder select est;
        }

        public IEnumerable<IEmploymentStatusType> AllEmploymentStatusTypes()
        {
            return from est in _db.EmploymentStatusTypes orderby est.SortOrder select est;
        }

        public IEmploymentStatusType EmploymentStatusTypeByName(string name)
        {
            return (from est in _db.EmploymentStatusTypes where est.Name.ToLower() == name.ToLower() select est).SingleOrDefault();
        }
    }
}
