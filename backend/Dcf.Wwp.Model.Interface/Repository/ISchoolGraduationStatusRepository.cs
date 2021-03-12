using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ISchoolGraduationStatusRepository
    {
        IEnumerable<ISchoolGraduationStatus> SchoolGraduationStatuses();
        IEnumerable<ISchoolGraduationStatus> AllSchoolGraduationStatuses();
        ISchoolGraduationStatus SchoolGraduationStatusByName(string name);
    }
}
