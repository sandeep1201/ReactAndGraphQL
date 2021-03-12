using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class HousingHistory : BaseCommonModel, IHousingHistory, IEquatable<HousingHistory>
    {
        IHousingSection IHousingHistory.HousingSection
        {
            get { return HousingSection; }
            set { HousingSection = (HousingSection) value; }
        }

        IHousingSituation IHousingHistory.HousingSituation
        {
            get { return HousingSituation; }
            set { HousingSituation = (HousingSituation) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var hh = new HousingHistory();
            hh.Id                 = this.Id;
            hh.HousingSituationId = this.HousingSituationId;
            hh.BeginDate          = this.BeginDate;
            hh.EndDate            = this.EndDate;
            hh.HousingSectionId   = this.HousingSectionId;
            hh.HasEvicted         = this.HasEvicted;
            hh.MonthlyAmount      = this.MonthlyAmount;
            hh.IsAmountUnknown    = this.IsAmountUnknown;
            hh.Details            = this.Details;
            hh.IsDeleted          = this.IsDeleted;
            hh.SortOrder          = this.SortOrder;
            hh.HousingSituation   = (HousingSituation) this.HousingSituation?.Clone();
            // NOTE: We don't clone references to "parent" objects such as HousingSection
            return hh;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as HousingHistory;
            return obj != null && Equals(obj);
        }

        public bool Equals(HousingHistory other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(HousingSituationId, other.HousingSituationId))
                return false;
            if (!AdvEqual(BeginDate, other.BeginDate))
                return false;
            if (!AdvEqual(EndDate, other.EndDate))
                return false;
            if (!AdvEqual(HousingSectionId, other.HousingSectionId))
                return false;
            if (!AdvEqual(HasEvicted, other.HasEvicted))
                return false;
            if (!AdvEqual(IsAmountUnknown, other.IsAmountUnknown))
                return false;
            if (!AdvEqual(MonthlyAmount, other.MonthlyAmount))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
