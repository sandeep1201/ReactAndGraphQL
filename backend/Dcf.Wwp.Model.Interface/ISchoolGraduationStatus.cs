using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISchoolGraduationStatus:ICommonModel
    {
        String Name { get; set; }
		ICollection<IEducationSection> EducationSections { get; set; }
    }
}