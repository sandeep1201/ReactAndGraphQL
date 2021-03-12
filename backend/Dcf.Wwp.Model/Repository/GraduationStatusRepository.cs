using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IEnumerable<ISchoolGraduationStatus> SchoolGraduationStatuses()
        {
            return _db.SchoolGraduationStatuses.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<ISchoolGraduationStatus> AllSchoolGraduationStatuses()
        {
            return _db.SchoolGraduationStatuses.OrderBy(x => x.SortOrder);
        }

        public ISchoolGraduationStatus SchoolGraduationStatusByName(string name)
        {
            return _db.SchoolGraduationStatuses.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
