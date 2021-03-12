using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ISchool:ICommonModel
    {
         String Name { get; set; }
         String Street { get; set; }
         String City { get; set; }
         Int32? StateId { get; set; }
         Decimal? Latitude { get; set; }
         Decimal? Longitude { get; set; }
         ICollection<IInvolvedWorkProgram> EducationSections { get; set; }
         IState State { get; set; }



    }
}