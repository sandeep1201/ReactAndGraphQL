using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ExamEquivalencyType : BaseEntity, IExamEquivalencyType, IEquatable<ExamEquivalencyType>
    {
        ICollection<IExamResult> IExamEquivalencyType.ExamResults
        {
            get { return ExamResults.Cast<IExamResult>().ToList(); }
            set { ExamResults = value.Cast<ExamResult>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var eet = new ExamEquivalencyType();
            eet.Id        = this.Id;
            eet.SortOrder = this.SortOrder;
            eet.Name      = this.Name;

            return eet;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ExamEquivalencyType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ExamEquivalencyType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
