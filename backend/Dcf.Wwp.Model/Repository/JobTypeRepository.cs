using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IJobTypeRepository
    {
        public IEnumerable<IJobType> JobTypes()
        {
            var r = _db.JobTypes
                       .OrderBy(i => i.SortOrder)
                       .Select(i => i);

            return (r);
        }

        public IJobType JobTypeById(int? id)
        {
            var r = _db.JobTypes.FirstOrDefault(i => i.Id == id);

            return (r);
        }

        public IJobType JobTypeByName(string name)
        {
            var r = _db.JobTypes.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());

            return (r);
        }

        public IQueryable<IJobType> GetJobTypes(Expression<Func<IJobType, bool>> clause)
        {
            var q = _db.JobTypes
                       .AsNoTracking()
                       .Where(clause);
                       //.ToList();

            return (q);
        }
    }
}
