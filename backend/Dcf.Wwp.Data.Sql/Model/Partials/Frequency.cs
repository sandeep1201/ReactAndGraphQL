using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Frequency : BaseCommonModel, IFrequency, IEquatable<Frequency>
    {
        ICollection<IActivityScheduleFrequencyBridge> IFrequency.MRActivityScheduleFrequencyBridges
        {
            get { return MRActivityScheduleFrequencyBridges.Cast<IActivityScheduleFrequencyBridge>().ToList(); }
            set { MRActivityScheduleFrequencyBridges = (ICollection<ActivityScheduleFrequencyBridge>) value; }
        }

        ICollection<IActivityScheduleFrequencyBridge> IFrequency.WKActivityScheduleFrequencyBridges
        {
            get { return WKActivityScheduleFrequencyBridges.Cast<IActivityScheduleFrequencyBridge>().ToList(); }
            set { WKActivityScheduleFrequencyBridges = (ICollection<ActivityScheduleFrequencyBridge>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var a = new Frequency();

            a.Id        = this.Id;
            a.Code      = this.Code;
            a.Name      = this.Name;
            a.SortOrder = this.SortOrder;
            a.IsDeleted = this.IsDeleted;
            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Frequency;
            return obj != null && Equals(obj);
        }

        public bool Equals(Frequency other)
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
            return true;
        }

        #endregion IEquatable<T>
    }
}
