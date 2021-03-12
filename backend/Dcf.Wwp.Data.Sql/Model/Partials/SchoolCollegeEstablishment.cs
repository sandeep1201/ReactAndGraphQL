using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class SchoolCollegeEstablishment : BaseCommonModel, ISchoolCollegeEstablishment, IEquatable<SchoolCollegeEstablishment>
    {
        ICollection<IPostSecondaryCollege> ISchoolCollegeEstablishment.PostSecondaryColleges
        {
            get => PostSecondaryColleges.Cast<IPostSecondaryCollege>().ToList();

            set => PostSecondaryColleges = value.Cast<PostSecondaryCollege>().ToList();
        }

        ICollection<IEducationSection> ISchoolCollegeEstablishment.EducationSections
        {
            get => EducationSections.Cast<IEducationSection>().ToList();

            set => EducationSections = value.Cast<EducationSection>().ToList();
        }

        ICity ISchoolCollegeEstablishment.City
        {
            get => City;
            set => City = (City) value;
        }

        #region ICloneable

        // Object's Properties without RowVersion and non-unqiue Cloned
        public object Clone()
        {
            var est = new SchoolCollegeEstablishment
                      {
                          Id        = Id,
                          Name      = Name,
                          Street    = Street,
                          CityId    = CityId,
                          City      = (City) City?.Clone()
                      };


            // NOTE: We don't clone references to "parent" objects such as Establishment

            return est;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as SchoolCollegeEstablishment;
            return obj != null && Equals(obj);
        }

        public bool Equals(SchoolCollegeEstablishment other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(Street, other.Street))
                return false;
            if (!AdvEqual(CityId, other.CityId))
                return false;
            if (!AdvEqual(City, other.City))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
