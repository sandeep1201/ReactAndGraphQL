using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IApplicationStatusTypeRepository
    {
        public IEnumerable<IApplicationStatusType> ApplicationStatusTypes()
        {
            return from x in _db.ApplicationStatusTypes where !x.IsDeleted orderby x.SortOrder select x;
        }

        public IApplicationStatusType ApplicationStatusTypeById(int? id)
        {
            var applicationStatusType = (from ct in _db.ApplicationStatusTypes where ct.Id == id select ct).FirstOrDefault();
            return applicationStatusType;
        }
    }
}
