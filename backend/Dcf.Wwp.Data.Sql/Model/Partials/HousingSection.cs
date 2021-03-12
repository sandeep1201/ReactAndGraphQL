using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class HousingSection : BaseCommonModel, IHousingSection, IEquatable<HousingSection>
    {
        ICollection<IHousingHistory> IHousingSection.HousingHistories
        {
            get { return (from x in HousingHistories where x.IsDeleted == false select x).Cast<IHousingHistory>().ToList(); }

            set { HousingHistories = value.Cast<HousingHistory>().ToList(); }
        }

        ICollection<IHousingHistory> IHousingSection.AllHousingHistories
        {
            get { return (HousingHistories).Cast<IHousingHistory>().ToList(); }

            set { HousingHistories = value.Cast<HousingHistory>().ToList(); }
        }

        IHousingSituation IHousingSection.HousingSituation
        {
            get { return HousingSituation; }
            set { HousingSituation = (HousingSituation) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var hs = new HousingSection();
            hs.Id                            = this.Id;
            hs.HousingSituationId            = this.HousingSituationId;
            hs.CurrentHousingDetails         = this.CurrentHousingDetails;
            hs.CurrentHousingBeginDate       = this.CurrentHousingBeginDate;
            hs.CurrentHousingEndDate         = this.CurrentHousingEndDate;
            hs.HasCurrentEvictionRisk        = this.HasCurrentEvictionRisk;
            hs.IsCurrentAmountUnknown        = this.IsCurrentAmountUnknown;
            hs.CurrentMonthlyAmount          = this.CurrentMonthlyAmount;
            hs.HasBeenEvicted                = this.HasBeenEvicted;
            hs.IsCurrentMovingToHistory      = this.IsCurrentMovingToHistory;
            hs.HasUtilityDisconnectionRisk   = this.HasUtilityDisconnectionRisk;
            hs.UtilityDisconnectionRiskNotes = this.UtilityDisconnectionRiskNotes;
            hs.HasDifficultyWorking          = this.HasDifficultyWorking;
            hs.DifficultyWorkingNotes        = this.DifficultyWorkingNotes;
            hs.Notes                         = this.Notes;
            hs.HousingHistories              = this.HousingHistories.Select(x => (HousingHistory) x.Clone()).ToList();
            hs.HousingSituation              = (HousingSituation) this.HousingSituation?.Clone();
            return hs;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as HousingSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(HousingSection other)
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
            if (!AdvEqual(CurrentHousingDetails, other.CurrentHousingDetails))
                return false;
            if (!AdvEqual(CurrentHousingBeginDate, other.CurrentHousingBeginDate))
                return false;
            if (!AdvEqual(CurrentHousingEndDate, other.CurrentHousingEndDate))
                return false;
            if (!AdvEqual(HasCurrentEvictionRisk, other.HasCurrentEvictionRisk))
                return false;
            if (!AdvEqual(IsCurrentAmountUnknown, other.IsCurrentAmountUnknown))
                return false;
            if (!AdvEqual(CurrentMonthlyAmount, other.CurrentMonthlyAmount))
                return false;
            if (!AdvEqual(HasBeenEvicted, other.HasBeenEvicted))
                return false;
            if (!AdvEqual(IsCurrentMovingToHistory, other.IsCurrentMovingToHistory))
                return false;
            if (!AdvEqual(HasUtilityDisconnectionRisk, other.HasUtilityDisconnectionRisk))
                return false;
            if (!AdvEqual(UtilityDisconnectionRiskNotes, other.UtilityDisconnectionRiskNotes))
                return false;
            if (!AdvEqual(HasDifficultyWorking, other.HasDifficultyWorking))
                return false;
            if (!AdvEqual(DifficultyWorkingNotes, other.DifficultyWorkingNotes))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;

            var areBothNotNull    = AreBothNotNull(HousingHistories, other.HousingHistories);
            var areSequencesEqual = HousingHistories.OrderBy(x => x.Id).SequenceEqual(other.HousingHistories.OrderBy(x => x.Id));

            if (areBothNotNull && !(areSequencesEqual))
            {
                return false;
            }

            //var h  = HousingHistories.OrderBy(x => x.Id);
            //var o  = other.HousingHistories.OrderBy(x => x.Id);
            //var nn = AreBothNotNull(h, o);
            //var se = h.SequenceEqual(o);

            //if (nn && !(se))
            //{
            //    return false;
            //}

            return true;
        }

        #endregion IEquatable<T>
    }
}
