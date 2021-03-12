using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface ISchoolGradeLevel : ICommonModel
    {
         Int32 SortOrder { get; set; }
         Int32? Grade { get; set; }
         String Name { get; set; }
        ICollection<IEducationSection> EducationSections { get; set; }
    }
}
