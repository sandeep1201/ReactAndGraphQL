using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class SchoolGraduationStatus : BaseCommonModel, ISchoolGraduationStatus, IEquatable<SchoolGraduationStatus>
    {
        ICollection<IEducationSection> ISchoolGraduationStatus.EducationSections
        {
            get => EducationSections.Cast<IEducationSection>().ToList();
            set => EducationSections = value.Cast<EducationSection>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var sgs = new SchoolGraduationStatus();

            sgs.Id   = Id;
            sgs.Name = Name;

            return sgs;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as SchoolGradeLevel;
            return obj != null && Equals(obj);
        }

        public bool Equals(SchoolGraduationStatus other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
