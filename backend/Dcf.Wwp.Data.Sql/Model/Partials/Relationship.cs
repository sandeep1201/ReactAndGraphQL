using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Relationship : BaseCommonModel, IRelationship, IEquatable<Relationship>
    {
        ICollection<IFamilyMember> IRelationship.FamilyMembers
        {
            get => FamilyMembers.Cast<IFamilyMember>().ToList();
            set => FamilyMembers = value.Cast<FamilyMember>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new Relationship()
                        {
                            Id           = Id,
                            RelationName = RelationName,
                            IsDeleted    = IsDeleted
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Relationship;
            return obj != null && Equals(obj);
        }

        public bool Equals(Relationship other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(RelationName, other.RelationName))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
