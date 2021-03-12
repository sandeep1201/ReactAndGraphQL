using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    partial class NonCustodialParentRelationship : BaseCommonModel, INonCustodialParentRelationship, IEquatable<NonCustodialParentRelationship>
    {
        ICollection<INonCustodialCaretaker> INonCustodialParentRelationship.NonCustodialCaretakers
        {
            get { return NonCustodialCaretakers.Cast<INonCustodialCaretaker>().ToList(); }
            set { NonCustodialCaretakers = value.Cast<NonCustodialCaretaker>().ToList(); }
        }


        #region ICloneable

        public object Clone()
        {
            var clone = new NonCustodialParentRelationship()
                        {
                            Id        = this.Id,
                            Name      = this.Name,
                            SortOrder = this.SortOrder,
                            IsDeleted = this.IsDeleted,

                            //AgeCategory = (AgeCategory)this.AgeCategory?.Clone(),
                            //Child = (Child)this.Child?.Clone(),
                            //ChildCareArrangement = (ChildCareArrangement)this.ChildCareArrangement?.Clone(),

                            // Don't clone parent object -- qill cause recursive calls.
                            //ChildYouthSection = (ChildYouthSection)this.ChildYouthSection?.Clone(),
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonCustodialParentRelationship;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonCustodialParentRelationship other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            //if (!AdvEqual(AgeCategory, other.AgeCategory))
            //    return false;
            //if (!AdvEqual(Child, other.Child))
            //    return false;
            //if (!AdvEqual(ChildCareArrangement, other.ChildCareArrangement))
            //    return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
