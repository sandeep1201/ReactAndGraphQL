using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyMember : BaseEntity, IFamilyMember, IEquatable<FamilyMember>
    {
        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }

            set { DeleteReason = (DeleteReason) value; }
        }

        IFamilyBarriersSection IFamilyMember.FamilyBarriersSection
        {
            get { return FamilyBarriersSection; }
            set { FamilyBarriersSection = (FamilyBarriersSection) value; }
        }

        IRelationship IFamilyMember.Relationship
        {
            get { return Relationship; }
            set { Relationship = (Relationship) value; }
        }


        #region ICloneable

        public object Clone()
        {
            var clone = new FamilyMember
                        {
                            Id                      = this.Id,
                            FamilyBarriersSectionId = this.FamilyBarriersSectionId,
                            RelationshipId          = this.RelationshipId,
                            FirstName               = this.FirstName,
                            LastName                = this.LastName,
                            Details                 = this.Details,
                            DeleteReasonId          = this.DeleteReasonId,
                            ModifiedDate            = this.ModifiedDate,
                            ModifiedBy              = this.ModifiedBy,
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as FamilyMember;
            return obj != null && Equals(obj);
        }

        public bool Equals(FamilyMember other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(FamilyBarriersSectionId, other.FamilyBarriersSectionId))
                return false;
            if (!AdvEqual(RelationshipId, other.RelationshipId))
                return false;
            if (!AdvEqual(FirstName, other.FirstName))
                return false;
            if (!AdvEqual(LastName, other.LastName))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
