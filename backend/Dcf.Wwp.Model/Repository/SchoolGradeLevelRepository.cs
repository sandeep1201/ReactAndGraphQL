using System.Collections.Generic;
using System.Linq;

using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public ISchoolGradeLevel SchoolGradeLevelByGrade(int grade)
        {
            var schoolGradelevel = (from sg in _db.SchoolGradeLevels where sg.Grade == grade select sg).SingleOrDefault();

            return schoolGradelevel;
        }

        public ISchoolGradeLevel SchoolGradeLevelById(int? id)
        {
            var schoolGradelevel = (from sg in _db.SchoolGradeLevels where sg.Id == id select sg).SingleOrDefault();

            return schoolGradelevel;
        }

        public IEnumerable<ISchoolGradeLevel> SchoolGradeLevels()
        {
            return _db.SchoolGradeLevels.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<ISchoolGradeLevel> AllSchoolGradeLevels()
        {
            return _db.SchoolGradeLevels.OrderBy(x => x.SortOrder);
        }
        
    }
}
