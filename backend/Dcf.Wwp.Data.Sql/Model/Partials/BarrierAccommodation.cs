using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierAccommodation : BaseEntity, IBarrierAccommodation, IEquatable<BarrierAccommodation>
    {
        /// <summary>
        ///     Accommodation is considered open if it has no end date and isn't soft deleted.
        /// </summary>
        public bool IsOpen => EndDate == null && DeleteReasonId == null;

        IAccommodation IBarrierAccommodation.Accommodation
        {
            get => Accommodation;
            set => Accommodation = (Accommodation) value;
        }

        IBarrierDetail IBarrierAccommodation.BarrierDetail
        {
            get => BarrierDetail;
            set => BarrierDetail = (BarrierDetail) value;
        }

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get => DeleteReason;
            set => DeleteReason = (DeleteReason) value;
        }

        #region ICloneable

        public object Clone()
        {
            var ba = new BarrierAccommodation();
            ba.Id              = Id;
            ba.AccommodationId = AccommodationId;
            ba.BeginDate       = BeginDate;
            ba.EndDate         = EndDate;
            ba.Details         = Details;
            ba.DeleteReasonId  = DeleteReasonId;

            return ba;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as BarrierAccommodation;

            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierAccommodation other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(AccommodationId, other.AccommodationId))
            {
                return false;
            }

            if (!AdvEqual(BeginDate, other.BeginDate))
            {
                return false;
            }

            if (!AdvEqual(EndDate, other.EndDate))
            {
                return false;
            }

            if (!AdvEqual(Details, other.Details))
            {
                return false;
            }

            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
