using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICity : ICommonDelModel
    {
        String Name          { get; set; }
        String GooglePlaceId { get; set; }
        Int32? StateId       { get; set; }

        // Boolean IsDeleted { get; set; }
        Int32? CountryId { get; set; }

        Decimal? LatitudeNumber  { get; set; }
        Decimal? LongitudeNumber { get; set; }

        IState                                   State                       { get; set; }
        ICollection<ISchoolCollegeEstablishment> SchoolCollegeEstablishments { get; set; }

        ICollection<IInvolvedWorkProgram>   InvolvedWorkPrograms   { get; set; }
        ICountry                            Country                { get; set; }
        ICollection<IEmploymentInformation> EmploymentInformations { get; set; }
        ICollection<INonSelfDirectedActivity>  SelfDirectedActivities { get; set; }
    }
}
