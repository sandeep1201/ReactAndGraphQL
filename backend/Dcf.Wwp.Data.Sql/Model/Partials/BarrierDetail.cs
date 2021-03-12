using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierDetail : BaseCommonModel, IBarrierDetail, IEquatable<BarrierDetail>
    {
        /// <summary>
        ///  A barrier is considered open if it has no end date and isn't soft deleted.
        /// </summary>
        public bool IsOpen => EndDate == null && IsDeleted == false;

        ICollection<IBarrierAccommodation> IBarrierDetail.BarrierAccommodations
        {
            get { return BarrierAccommodations.Cast<IBarrierAccommodation>().ToList(); }
            set { BarrierAccommodations = value.Cast<BarrierAccommodation>().ToList(); }
        }

        ICollection<IBarrierAccommodation> IBarrierDetail.NonDeletedBarrierAccommodations => (from x in BarrierAccommodations where x.DeleteReasonId == null select x).Cast<IBarrierAccommodation>().ToList();

        IBarrierSection IBarrierDetail.BarrierSection
        {
            get { return BarrierSection; }
            set { BarrierSection = (BarrierSection) value; }
        }

        IBarrierType IBarrierDetail.BarrierType
        {
            get { return BarrierType; }
            set { BarrierType = (BarrierType) value; }
        }

        IParticipant IBarrierDetail.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        ICollection<IBarrierDetailContactBridge> IBarrierDetail.BarrierDetailContactBridges
        {
            get { return BarrierDetailContactBridges.Cast<IBarrierDetailContactBridge>().ToList(); }
            set { BarrierDetailContactBridges = value.Cast<BarrierDetailContactBridge>().ToList(); }
        }

        ICollection<IFormalAssessment> IBarrierDetail.FormalAssessments
        {
            get { return FormalAssessments.Cast<IFormalAssessment>().ToList(); }
            set { FormalAssessments = (ICollection<FormalAssessment>) value; }
        }

        ICollection<IFormalAssessment> IBarrierDetail.               NonDeletedFormalAssessments                => (from x in FormalAssessments where x.DeleteReasonId == null select x).Cast<IFormalAssessment>().ToList();
        ICollection<IBarrierTypeBarrierSubTypeBridge> IBarrierDetail.NonDeletedBarrierTypeBarrierSubTypeBridges => (from x in BarrierTypeBarrierSubTypeBridges where !x.IsDeleted select x).Cast<IBarrierTypeBarrierSubTypeBridge>().ToList();
        ICollection<IBarrierDetailContactBridge> IBarrierDetail.     NonBarrierDetailContactBridges             => (from x in BarrierDetailContactBridges where !x.IsDeleted select x).Cast<IBarrierDetailContactBridge>().ToList();

        #region ICloneable

        ICollection<IBarrierTypeBarrierSubTypeBridge> IBarrierDetail.BarrierTypeBarrierSubTypeBridges
        {
            get { return BarrierTypeBarrierSubTypeBridges.Cast<IBarrierTypeBarrierSubTypeBridge>().ToList(); }
            set { BarrierTypeBarrierSubTypeBridges = (ICollection<BarrierTypeBarrierSubTypeBridge>) value; }
        }

        public object Clone()
        {
            var em = new BarrierDetail();

            em.Id                               = this.Id;
            em.ParticipantId                    = this.ParticipantId;
            em.BarrierTypeId                    = this.BarrierTypeId;
            em.OnsetDate                        = this.OnsetDate;
            em.EndDate                          = this.EndDate;
            em.IsAccommodationNeeded            = this.IsAccommodationNeeded;
            em.Details                          = this.Details;
            em.BarrierType                      = (BarrierType) this.BarrierType?.Clone();
            em.BarrierTypeBarrierSubTypeBridges = this.BarrierTypeBarrierSubTypeBridges?.Where(x => !x.IsDeleted).Select(x => (BarrierTypeBarrierSubTypeBridge) x.Clone()).ToList();
            em.BarrierDetailContactBridges      = this.BarrierDetailContactBridges?.Select(x => (BarrierDetailContactBridge) x.Clone()).ToList();
            em.BarrierAccommodations            = this.BarrierAccommodations?.Select(x => (BarrierAccommodation) x.Clone()).ToList();
            em.FormalAssessments                = this.FormalAssessments?.Select(x => (FormalAssessment) x.Clone()).ToList();
            em.IsConverted                      = this.IsConverted;
            return em;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as BarrierDetail;
            return obj != null && Equals(obj);
        }

        public bool Equals(BarrierDetail other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal

            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(BarrierTypeId, other.BarrierTypeId))
                return false;
            if (!AdvEqual(OnsetDate, other.OnsetDate))
                return false;
            if (!AdvEqual(EndDate, other.EndDate))
                return false;
            if (!AdvEqual(IsAccommodationNeeded, other.IsAccommodationNeeded))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (AreBothNotNull(BarrierAccommodations, other.BarrierAccommodations) && !(BarrierAccommodations.OrderBy(x => x.Id).SequenceEqual(other.BarrierAccommodations.OrderBy(x => x.Id))))
                return false;
            if (AreBothNotNull(FormalAssessments, other.FormalAssessments) && !FormalAssessments.OrderBy(x => x.Id).SequenceEqual(other.FormalAssessments.OrderBy(x => x.Id)))
                return false;
            if (AreBothNotNull(BarrierDetailContactBridges, other.BarrierDetailContactBridges) && !BarrierDetailContactBridges.OrderBy(x => x.Id).SequenceEqual(other.BarrierDetailContactBridges.OrderBy(x => x.Id)))
                return false;
            if (AreBothNotNull(BarrierTypeBarrierSubTypeBridges, other.BarrierTypeBarrierSubTypeBridges) && !BarrierTypeBarrierSubTypeBridges.OrderBy(x => x.Id).SequenceEqual(other.BarrierTypeBarrierSubTypeBridges.OrderBy(x => x.Id)))
                return false;
            if (!AdvEqual(IsConverted, other.IsConverted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
