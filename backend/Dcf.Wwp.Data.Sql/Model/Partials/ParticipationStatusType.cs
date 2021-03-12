using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipationStatusType : BaseCommonModel, IParticipationStatusType, IEquatable<ParticipationStatusType>
    {
        ICollection<IParticipationStatu> IParticipationStatusType.ParticipationStatus
        {
            get { return ParticipationStatus.Cast<IParticipationStatu>().ToList(); }
            set { ParticipationStatus = (ICollection<ParticipationStatu>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var at = new ParticipationStatusType();

            at.Id            = this.Id;
            at.Code          = this.Code;
            at.Name          = this.Name;
            at.SortOrder     = this.SortOrder;
            at.IsDeleted     = this.IsDeleted;
            at.EffectiveDate = this.EffectiveDate;
            at.EndDate       = this.EndDate;
            return at;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ParticipationStatusType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ParticipationStatusType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Code, other.Code))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(EffectiveDate, other.EffectiveDate))
                return false;
            if (!AdvEqual(EndDate, other.EndDate))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
