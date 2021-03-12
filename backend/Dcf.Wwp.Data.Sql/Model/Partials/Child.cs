using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Child : BaseCommonModel, IChild, IEquatable<Child>
    {
        ICollection<IChildYouthSectionChild> IChild.ChildYouthSectionChilds
        {
            get { return ChildYouthSectionChilds.Cast<IChildYouthSectionChild>().ToList(); }
            set { ChildYouthSectionChilds = value.Cast<ChildYouthSectionChild>().ToList(); }
        }

        IGenderType IChild.GenderType
        {
            get { return GenderType; }
            set { GenderType = (GenderType) value; }
        }

        ICollection<IParticipantChildRelationshipBridge> IChild.ParticipantChildRelationshipBridges
        {
            get { return ParticipantChildRelationshipBridges.Cast<IParticipantChildRelationshipBridge>().ToList(); }
            set { ParticipantChildRelationshipBridges = value.Cast<ParticipantChildRelationshipBridge>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new Child
                        {
                            Id                = this.Id,
                            PinNumber         = this.PinNumber,
                            FirstName         = this.FirstName,
                            MiddleInitialName = this.MiddleInitialName,
                            LastName          = this.LastName,
                            SuffixName        = this.SuffixName,
                            DateOfBirth       = this.DateOfBirth,
                            DateOfDeath       = this.DateOfDeath,
                            GenderIndicator   = this.GenderIndicator,
                            IsDeleted         = this.IsDeleted
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Child;
            return obj != null && Equals(obj);
        }

        public bool Equals(Child other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(PinNumber, other.PinNumber))
                return false;
            if (!AdvEqual(FirstName, other.FirstName))
                return false;
            if (!AdvEqual(MiddleInitialName, other.MiddleInitialName))
                return false;
            if (!AdvEqual(LastName, other.LastName))
                return false;
            if (!AdvEqual(SuffixName, other.SuffixName))
                return false;
            if (!AdvEqual(DateOfBirth, other.DateOfBirth))
                return false;
            if (!AdvEqual(DateOfDeath, other.DateOfDeath))
                return false;
            if (!AdvEqual(GenderIndicator, other.GenderIndicator))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
