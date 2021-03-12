using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ISchoolGradeLevelRepository
    {
        ISchoolGradeLevel SchoolGradeLevelByGrade(int grade);

        ISchoolGradeLevel SchoolGradeLevelById(int? id);

        IEnumerable<ISchoolGradeLevel> SchoolGradeLevels();
        IEnumerable<ISchoolGradeLevel> AllSchoolGradeLevels();
    }
}
