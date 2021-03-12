using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ISchoolCollegeEstablishmentRepository
    {
        public ISchoolCollegeEstablishment SchoolByNameStreetCityStateCodeCountry(string name, string street, string city, string stateCode, string country)
        {
            var school = (from s in _db.SchoolCollegeEstablishments where s.Name == name & s.Street == street & s.City.Name == city & s.City.State.Code == stateCode & s.City.State.Country.Name == country select s).FirstOrDefault();
            return school;
        }

        public ISchoolCollegeEstablishment SchoolByNameStreetCityCountry(string name, string street, string city, string country)
        {
            var school = (from s in _db.SchoolCollegeEstablishments where s.Name == name & s.Street == street & s.City.Name == city & s.City.Country.Name == country select s).FirstOrDefault();
            return school;
        }

        public ISchoolCollegeEstablishment SchoolByNameStreet(string name, string street)
        {
            var school = (from s in _db.SchoolCollegeEstablishments where s.Name == name & s.Street == street select s).FirstOrDefault();
            return school;
        }

        public ISchoolCollegeEstablishment NewSchoolByEducationSection(IEducationSection parentSection, string user)
        {
            var s = new SchoolCollegeEstablishment();

            parentSection.SchoolCollegeEstablishment = s;
            s.ModifiedDate                           = DateTime.Now;
            s.ModifiedBy                             = user;
            _db.SchoolCollegeEstablishments.Add(s);
            return s;
        }

        public ISchoolCollegeEstablishment NewSchoolByPostSecondaryEducation(IPostSecondaryCollege parentSection, string user)
        {
            var s = new SchoolCollegeEstablishment();

            parentSection.SchoolCollegeEstablishment = s;
            s.ModifiedDate                           = DateTime.Now;
            s.ModifiedBy                             = user;
            _db.SchoolCollegeEstablishments.Add(s);
            return s;
        }

        public ISchoolCollegeEstablishment SchoolById(int? id)
        {
            var school = (from s in _db.SchoolCollegeEstablishments where s.Id == id select s).SingleOrDefault();
            return school;
        }

        public IEnumerable<ISchoolCollegeEstablishment> AllSchools()
        {
            return _db.SchoolCollegeEstablishments.OrderBy(x => x.Name);
        }
    }
}
