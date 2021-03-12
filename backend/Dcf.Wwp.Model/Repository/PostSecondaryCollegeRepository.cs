using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IPostSecondaryCollegeRepository
    {
        public IEnumerable<IPostSecondaryCollege> AllPostSecondaryColleges()
        {
            return _db.PostSecondaryColleges.OrderBy(x => x.SortOrder);
        }

        public IPostSecondaryCollege NewCollege(IPostSecondaryEducationSection parentSection, string user)
        {
            IPostSecondaryCollege psc = new PostSecondaryCollege();
            psc.PostSecondaryEducationSection = parentSection;
            psc.ModifiedDate = DateTime.Now;
            psc.ModifiedBy = user;
            psc.IsDeleted = false;
            _db.PostSecondaryColleges.Add((PostSecondaryCollege)psc);

            return psc;
        }
    }
}
