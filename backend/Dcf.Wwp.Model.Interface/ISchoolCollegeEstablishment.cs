using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISchoolCollegeEstablishment : ICommonModel
    {
        String  Name      { get; set; }
        String  Street    { get; set; }
        Int32?  CityId    { get; set; }
        Boolean IsDeleted { get; set; }

        ICity City { get; set; }

        ICollection<IEducationSection> EducationSections { get; set; }

        ICollection<IPostSecondaryCollege> PostSecondaryColleges { get; set; }
    }
}
