using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IJobTypeRepository
    {
        IEnumerable<IJobType> JobTypes();
        IJobType JobTypeById(Int32? id);
        IJobType JobTypeByName(String name);
        IQueryable<IJobType> GetJobTypes(Expression<Func<IJobType, bool>> clause);
    }
}
