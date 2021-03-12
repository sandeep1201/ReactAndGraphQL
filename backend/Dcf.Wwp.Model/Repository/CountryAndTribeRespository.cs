using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ICountyAndTribeRepository
    {
        public IEnumerable<ICountyAndTribe> GetCounties()
        {
            return _db.CountyAndTribes.Where(x => !x.IsDeleted && x.IsCounty).OrderBy(x => x.CountyName);
        }

        public IEnumerable<ICountyAndTribe> GetTribes()
        {
            return _db.CountyAndTribes.Where(x => !x.IsDeleted && !x.IsCounty).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<ICountyAndTribe> GetCountyAndTribes()
        {
            return _db.CountyAndTribes.Where(x => !x.IsDeleted).OrderBy(x => x.CountyName);
        }

        public IEnumerable<ICountyAndTribe> WhereCountyAndTribe(Expression<Func<ICountyAndTribe, bool>> clause)
        {
            try
            {
                return _db.CountyAndTribes
                          .Where(clause)
                          .AsNoTracking();
                //.ToList();
            }
            catch (NullReferenceException)
            {
                return new List<ICountyAndTribe>();
            }
        }

        public ICountyAndTribe GetCountyOrTribe(Expression<Func<ICountyAndTribe, bool>> clause)
        {
            return _db.CountyAndTribes.FirstOrDefault(clause);
        }

        public ICountyAndTribe GetCountyOrTribeById(long id)
        {
            return _db.CountyAndTribes.Find(id);
        }
    }
}
