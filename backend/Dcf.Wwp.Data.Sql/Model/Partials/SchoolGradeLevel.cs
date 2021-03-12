using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class SchoolGradeLevel : BaseCommonModel, ISchoolGradeLevel, IEquatable<SchoolGradeLevel>
    {
        ICollection<IEducationSection> ISchoolGradeLevel.EducationSections
        {
            get { return EducationSections.Cast<IEducationSection>().ToList(); }
            set { EducationSections = value.Cast<EducationSection>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var sgl = new SchoolGradeLevel();

            sgl.Id        = this.Id;
            sgl.Name      = this.Name;
            sgl.SortOrder = this.SortOrder;
            sgl.Grade     = this.Grade;

            return sgl;
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

        public bool Equals(SchoolGradeLevel other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)               &&
                   SortOrder.Equals(other.SortOrder) &&
                   Name.Equals(other.Name)           &&
                   Grade.Equals(other.Grade);
        }

        #endregion IEquatable<T>
    }
}
