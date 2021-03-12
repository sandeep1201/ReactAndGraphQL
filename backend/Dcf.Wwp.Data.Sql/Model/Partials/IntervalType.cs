using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class IntervalType : BaseCommonModel, IIntervalType, IEquatable<IntervalType>
    {
        ICollection<IWageHour> IIntervalType.CurrentPayRateIntervalTypes
        {
            get { return CurrentPayRateIntervalTypes.Cast<IWageHour>().ToList(); }
            set { CurrentPayRateIntervalTypes = value.Cast<WageHour>().ToList(); }
        }

        ICollection<IWageHour> IIntervalType.BeginRateIntervalTypes
        {
            get { return BeginRateIntervalTypes.Cast<IWageHour>().ToList(); }
            set { BeginRateIntervalTypes = value.Cast<WageHour>().ToList(); }
        }

        ICollection<IWageHour> IIntervalType.EndRateIntervalTypes
        {
            get { return EndRateIntervalTypes.Cast<IWageHour>().ToList(); }
            set { EndRateIntervalTypes = value.Cast<WageHour>().ToList(); }
        }

        ICollection<IWageHourHistory> IIntervalType.WageHourHistories
        {
            get { return WageHourHistories.Cast<IWageHourHistory>().ToList(); }
            set { WageHourHistories = value.Cast<WageHourHistory>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var it = new IntervalType();

            it.Id        = this.Id;
            it.SortOrder = this.SortOrder;
            it.Name      = this.Name;
            return it;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as IntervalType;
            return obj != null && Equals(obj);
        }

        public bool Equals(IntervalType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)               &&
                   SortOrder.Equals(other.SortOrder) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
