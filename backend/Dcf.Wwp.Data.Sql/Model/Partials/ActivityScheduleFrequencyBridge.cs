using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityScheduleFrequencyBridge : BaseCommonModel, IActivityScheduleFrequencyBridge, IEquatable<ActivityScheduleFrequencyBridge>
    {
        IActivitySchedule IActivityScheduleFrequencyBridge.ActivitySchedule
        {
            get => ActivitySchedule;
            set => ActivitySchedule = (ActivitySchedule) value;
        }

        IFrequency IActivityScheduleFrequencyBridge.MRFrequency
        {
            get => MRFrequency;
            set => MRFrequency = (Frequency) value;
        }

        IFrequency IActivityScheduleFrequencyBridge.WKFrequency
        {
            get => WKFrequency;
            set => WKFrequency = (Frequency) value;
        }

        #region ICloneable

        public object Clone()
        {
            var a = new ActivityScheduleFrequencyBridge();

            a.Id                 = Id;
            a.ActivityScheduleId = ActivityScheduleId;
            a.WKFrequencyId      = WKFrequencyId;
            a.MRFrequencyId      = MRFrequencyId;
            a.IsDeleted          = IsDeleted;

            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as ActivityScheduleFrequencyBridge;

            return obj != null && Equals(obj);
        }

        public bool Equals(ActivityScheduleFrequencyBridge other)
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

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(ActivityScheduleId, other.ActivityScheduleId))
            {
                return false;
            }

            if (!AdvEqual(WKFrequencyId, other.WKFrequencyId))
            {
                return false;
            }

            if (!AdvEqual(MRFrequencyId, other.MRFrequencyId))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
