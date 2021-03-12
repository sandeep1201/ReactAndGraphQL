using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonSelfDirectedActivity : BaseCommonModel, INonSelfDirectedActivity, IEquatable<NonSelfDirectedActivity>
    {
        IActivity INonSelfDirectedActivity.Activity
        {
            get { return Activity; }
            set { Activity = (Activity) value; }
        }

        ICity INonSelfDirectedActivity.City
        {
            get { return City; }
            set { City = (City) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var a = new NonSelfDirectedActivity();

            a.Id            = this.Id;
            a.ActivityId    = this.ActivityId;
            a.BusinessName  = this.BusinessName;
            a.CityId        = this.CityId;
            a.PhoneNumber   = this.PhoneNumber;
            a.StreetAddress = this.StreetAddress;
            a.ZipAddress    = this.ZipAddress;
            a.IsDeleted     = this.IsDeleted;
            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as NonSelfDirectedActivity;
            return obj != null && Equals(obj);
        }

        public bool Equals(NonSelfDirectedActivity other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ActivityId, other.ActivityId))
                return false;
            if (!AdvEqual(BusinessName, other.BusinessName))
                return false;
            if (!AdvEqual(CityId, other.CityId))
                return false;
            if (!AdvEqual(PhoneNumber, other.PhoneNumber))
                return false;
            if (!AdvEqual(StreetAddress, other.StreetAddress))
                return false;
            if (!AdvEqual(ZipAddress, other.ZipAddress))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
