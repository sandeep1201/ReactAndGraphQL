using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ISchoolCollegeEstablishmentRepository
    {
        ISchoolCollegeEstablishment SchoolByNameStreetCityStateCodeCountry(string name, string street, string city, string stateCode, string country);

        ISchoolCollegeEstablishment SchoolByNameStreetCityCountry(string name, string street, string city, string country);
        ISchoolCollegeEstablishment NewSchoolByEducationSection(IEducationSection parentSection, string user);
        ISchoolCollegeEstablishment NewSchoolByPostSecondaryEducation(IPostSecondaryCollege parentSection, string user);

        ISchoolCollegeEstablishment SchoolByNameStreet(string name, string street);

        ISchoolCollegeEstablishment SchoolById(int? id);

        IEnumerable<ISchoolCollegeEstablishment> AllSchools();
    }
}
