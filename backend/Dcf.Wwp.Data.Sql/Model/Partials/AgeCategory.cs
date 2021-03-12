using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AgeCategory : IAgeCategory, IEquatable<AgeCategory>
    {
        ICollection<IChildYouthSectionChild> IAgeCategory.ChildYouthSectionChilds
        {
            get { return ChildYouthSectionChilds.Cast<IChildYouthSectionChild>().ToList(); }
            set { ChildYouthSectionChilds = value.Cast<ChildYouthSectionChild>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new AgeCategory();

            clone.Id              = this.Id;
            clone.AgeRange        = this.AgeRange;
            clone.DescriptionText = this.DescriptionText;

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as AgeCategory;
            return obj != null && Equals(obj);
        }

        public bool Equals(AgeCategory other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(AgeRange, other.AgeRange))
                return false;
            if (!AdvEqual(DescriptionText, other.DescriptionText))
                return false;
            return true;
        }

        protected bool AdvEqual(object obj1, object obj2)
        {
            if (obj1 == null ^ obj2 == null)
            {
                return false;
            }

            if (obj1 == null && obj2 == null)
            {
                return true;
            }

            if (obj1.Equals(obj2))
            {
                return true;
            }

            return false;
        }

        #endregion IEquatable<T>
    }
}
