using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityType : BaseCommonModel, IActivityType, IEquatable<ActivityType>
    {
        ICollection<IActivity> IActivityType.Activities
        {
            get { return Activities.Cast<IActivity>().ToList(); }
            set { Activities = (ICollection<Activity>) value; }
        }

        ICollection<IEnrolledProgramEPActivityTypeBridge> IActivityType.EnrolledProgramEPActivityTypeBridges
        {
            get { return EnrolledProgramEPActivityTypeBridges.Cast<IEnrolledProgramEPActivityTypeBridge>().ToList(); }
            set { EnrolledProgramEPActivityTypeBridges = (ICollection<EnrolledProgramEPActivityTypeBridge>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var at = new ActivityType();

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

            var obj = other as ActivityType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ActivityType other)
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
