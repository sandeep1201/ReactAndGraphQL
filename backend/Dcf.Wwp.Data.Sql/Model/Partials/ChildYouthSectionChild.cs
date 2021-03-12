using System;
using System.Diagnostics;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ChildYouthSectionChild : BaseEntity, IChildYouthSectionChild, IEquatable<ChildYouthSectionChild>
    {
        IAgeCategory IChildYouthSectionChild.AgeCategory
        {
            get => AgeCategory;
            set => AgeCategory = (AgeCategory) value;
        }

        IChild IChildYouthSectionChild.Child
        {
            get => Child;
            set => Child = (Child) value;
        }

        IChildCareArrangement IChildYouthSectionChild.ChildCareArrangement
        {
            get => ChildCareArrangement;
            set => ChildCareArrangement = (ChildCareArrangement) value;
        }

        // This parent objects should not be used for cloning as it will cause recursive
        // calls (bad).  We do need it though since a whole new object graph could be
        // created and we don't yet have an ID.
        IChildYouthSection IChildYouthSectionChild.ChildYouthSection
        {
            get => ChildYouthSection;
            set => ChildYouthSection = (ChildYouthSection) value;
        }

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get => DeleteReason;
            set => DeleteReason = (DeleteReason) value;
        }


        #region ICloneable

        public object Clone()
        {
            var clone = new ChildYouthSectionChild
                        {
                            Id                  = Id,
                            ChildYouthSectionId = ChildYouthSectionId,
                            ChildId             = ChildId,
                            CareArrangementId   = CareArrangementId,
                            AgeCategoryId       = AgeCategoryId,
                            IsSpecialNeeds      = IsSpecialNeeds,
                            Details             = Details,
                            DeleteReasonId      = DeleteReasonId,

                            AgeCategory          = (AgeCategory) AgeCategory?.Clone(),
                            Child                = (Child) Child?.Clone(),
                            ChildCareArrangement = (ChildCareArrangement) ChildCareArrangement?.Clone()
                            // TODO: Clone Del Reason
                            // DeleteReason = (DeleteReason)this.DeleteReason?.Clone(),

                            // Don't clone parent object -- qill cause recursive calls.
                            //ChildYouthSection = (ChildYouthSection)this.ChildYouthSection?.Clone(),
                        };

            return clone;
        }

        #endregion ICloneable

        #region IComplexModel

        public void SetModifiedOnComplexProperties<T>(T cloned, string user, DateTime modDate)
            where T : class, ICloneable, ICommonModel
        {
            // We don't need to set modified on null objects.
            if (cloned == null) return;

            Debug.Assert(cloned is IChildYouthSectionChild, "cloned is not IChildYouthSectionChild");

            var clone = (IChildYouthSectionChild) cloned;

            // Look at the complex Child property.
            if (!AdvEqual(Child, clone.Child))
            {
                ModifiedBy   = user;
                ModifiedDate = modDate;
            }
        }

        #endregion IComplexModel

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ChildYouthSectionChild;
            return obj != null && Equals(obj);
        }

        public bool Equals(ChildYouthSectionChild other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ChildYouthSectionId, other.ChildYouthSectionId))
                return false;
            if (!AdvEqual(ChildId, other.ChildId))
                return false;
            if (!AdvEqual(CareArrangementId, other.CareArrangementId))
                return false;
            if (!AdvEqual(AgeCategoryId, other.AgeCategoryId))
                return false;
            if (!AdvEqual(IsSpecialNeeds, other.IsSpecialNeeds))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;

            if (!AdvEqual(AgeCategory, other.AgeCategory))
                return false;
            if (!AdvEqual(Child, other.Child))
                return false;
            if (!AdvEqual(ChildCareArrangement, other.ChildCareArrangement))
                return false;
            if (!AdvEqual(DeleteReason, other.DeleteReason))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
