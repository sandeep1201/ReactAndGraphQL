using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityLocation : BaseCommonModel, IActivityLocation, IEquatable<ActivityLocation>
    {
        ICollection<IActivity> IActivityLocation.Activities
        {
            get => Activities.Cast<IActivity>().ToList();
            set => Activities = (ICollection<Activity>) value;
        }

        #region ICloneable

        public object Clone()
        {
            var al = new ActivityLocation();

            al.Id        = Id;
            al.Name      = Name;
            al.SortOrder = SortOrder;
            al.IsDeleted = IsDeleted;

            return al;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as ActivityLocation;

            return obj != null && Equals(obj);
        }

        public bool Equals(ActivityLocation other)
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

            if (!AdvEqual(Name, other.Name))
            {
                return false;
            }

            if (!AdvEqual(SortOrder, other.SortOrder))
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
