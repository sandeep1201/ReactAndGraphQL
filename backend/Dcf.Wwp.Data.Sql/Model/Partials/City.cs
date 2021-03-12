using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class City : BaseCommonModel, ICity, IEquatable<City>
    {
        IState ICity.State
        {
            get => State;
            set => State = (State) value;
        }

        ICountry ICity.Country
        {
            get => Country;
            set => Country = (Country) value;
        }


        ICollection<ISchoolCollegeEstablishment> ICity.SchoolCollegeEstablishments
        {
            get => SchoolCollegeEstablishments.Cast<ISchoolCollegeEstablishment>().ToList();
            set => SchoolCollegeEstablishments = value.Cast<SchoolCollegeEstablishment>().ToList();
        }

        ICollection<IInvolvedWorkProgram> ICity.InvolvedWorkPrograms
        {
            get => InvolvedWorkPrograms.Cast<IInvolvedWorkProgram>().ToList();
            set => InvolvedWorkPrograms = value.Cast<InvolvedWorkProgram>().ToList();
        }

        ICollection<IEmploymentInformation> ICity.EmploymentInformations
        {
            get => EmploymentInformations.Cast<IEmploymentInformation>().ToList();
            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        ICollection<INonSelfDirectedActivity> ICity.SelfDirectedActivities
        {
            get => NonSelfDirectedActivities.Cast<INonSelfDirectedActivity>().ToList();
            set => NonSelfDirectedActivities = value.Cast<NonSelfDirectedActivity>().ToList();
        }


        #region ICloneable

        public object Clone()
        {
            var c = new City();

            c.Id            = Id;
            c.Name          = Name;
            c.GooglePlaceId = GooglePlaceId;
            c.StateId       = StateId;
            c.CountryId     = CountryId;
            c.Country       = (Country) Country?.Clone();
            c.State         = (State) State?.Clone();
            return c;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as City;
            return obj != null && Equals(obj);
        }

        public bool Equals(City other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;


            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(GooglePlaceId, other.GooglePlaceId))
                return false;
            if (!AdvEqual(StateId, other.StateId))
                return false;
            if (!AdvEqual(CountryId, other.CountryId))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
