using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Activity : BaseCommonModel, IActivity, IEquatable<Activity>
    {
        IActivityLocation IActivity.ActivityLocation
        {
            get { return ActivityLocation; }
            set { ActivityLocation = (ActivityLocation) value; }
        }

        IActivityType IActivity.ActivityType
        {
            get { return ActivityType; }
            set { ActivityType = (ActivityType) value; }
        }

        ICollection<IActivitySchedule> IActivity.ActivitySchedules
        {
            get { return ActivitySchedules.Cast<IActivitySchedule>().ToList(); }
            set { ActivitySchedules = (ICollection<ActivitySchedule>) value; }
        }

        ICollection<INonSelfDirectedActivity> IActivity.SelfDirectedActivities
        {
            get { return NonSelfDirectedActivities.Cast<INonSelfDirectedActivity>().ToList(); }
            set { NonSelfDirectedActivities = (ICollection<NonSelfDirectedActivity>) value; }
        }

        ICollection<IActivityContactBridge> IActivity.ActivityContactBridges
        {
            get { return ActivityContactBridges.Cast<IActivityContactBridge>().ToList(); }
            set { ActivityContactBridges = value.Cast<ActivityContactBridge>().ToList(); }
        }

        ICollection<IActivityContactBridge> IActivity.NonActivityContactBridges => (from x in ActivityContactBridges where !x.IsDeleted select x).Cast<IActivityContactBridge>().ToList();

        ICollection<IEmployabilityPlanActivityBridge> IActivity.EmployabilityPlanActivityBridges
        {
            get { return EmployabilityPlanActivityBridges.Cast<IEmployabilityPlanActivityBridge>().ToList(); }
            set { EmployabilityPlanActivityBridges = value.Cast<EmployabilityPlanActivityBridge>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var a = new Activity();

            a.Id = this.Id;
            //a.EmployabilityPlanId = this.EmployabilityPlanId; //04/16/2019
            a.ActivityTypeId     = this.ActivityTypeId;
            a.Description        = this.Description;
            a.ActivityLocationId = this.ActivityLocationId;
            a.Details            = this.Details;
            a.IsDeleted          = this.IsDeleted;
            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Activity;
            return obj != null && Equals(obj);
        }

        public bool Equals(Activity other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            // if (!AdvEqual(EmployabilityPlanId, other.EmployabilityPlanId))
            //     return false;
            if (!AdvEqual(ActivityTypeId, other.ActivityTypeId))
                return false;
            if (!AdvEqual(Description, other.Description))
                return false;
            if (!AdvEqual(ActivityLocationId, other.ActivityLocationId))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
