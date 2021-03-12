using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivitySchedule : BaseCommonModel, IActivitySchedule, IEquatable<ActivitySchedule>
    {
        IActivity IActivitySchedule.Activity
        {
            get => Activity;
            set => Activity = (Activity) value;
        }

        IFrequencyType IActivitySchedule.FrequencyType
        {
            get => FrequencyType;
            set => FrequencyType = (FrequencyType) value;
        }

        ICollection<IActivityScheduleFrequencyBridge> IActivitySchedule.ActivityScheduleFrequencyBridges
        {
            get => ActivityScheduleFrequencyBridges.Cast<IActivityScheduleFrequencyBridge>().ToList();
            set => ActivityScheduleFrequencyBridges = (ICollection<ActivityScheduleFrequencyBridge>) value;
        }

        #region ICloneable

        public object Clone()
        {
            var a = new ActivitySchedule();

            a.Id              = Id;
            a.ActivityId      = ActivityId;
            a.StartDate       = StartDate;
            a.IsRecurring     = IsRecurring;
            a.FrequencyTypeId = FrequencyTypeId;
            a.PlannedEndDate  = PlannedEndDate;
            a.ActualEndDate   = ActualEndDate;
            a.HoursPerDay     = HoursPerDay;
            a.BeginTime       = BeginTime;
            a.EndTime         = EndTime;
            a.IsDeleted       = IsDeleted;

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

            var obj = other as ActivitySchedule;

            return obj != null && Equals(obj);
        }

        public bool Equals(ActivitySchedule other)
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

            if (!AdvEqual(ActivityId, other.ActivityId))
            {
                return false;
            }

            if (!AdvEqual(StartDate, other.StartDate))
            {
                return false;
            }

            if (!AdvEqual(IsRecurring, other.IsRecurring))
            {
                return false;
            }

            if (!AdvEqual(FrequencyTypeId, other.FrequencyTypeId))
            {
                return false;
            }

            if (!AdvEqual(PlannedEndDate, other.PlannedEndDate))
            {
                return false;
            }

            if (!AdvEqual(ActualEndDate, other.ActualEndDate))
            {
                return false;
            }

            if (!AdvEqual(HoursPerDay, other.HoursPerDay))
            {
                return false;
            }

            if (!AdvEqual(BeginTime, other.BeginTime))
            {
                return false;
            }

            if (!AdvEqual(EndTime, other.EndTime))
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
