using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FrequencyType : BaseCommonModel, IFrequencyType, IEquatable<FrequencyType>
    {
        ICollection<IActivitySchedule> IFrequencyType.ActivitySchedules
        {
            get { return ActivitySchedules.Cast<IActivitySchedule>().ToList(); }
            set { ActivitySchedules = (ICollection<ActivitySchedule>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var a = new FrequencyType();

            a.Id        = this.Id;
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

            var obj = other as FrequencyType;
            return obj != null && Equals(obj);
        }

        public bool Equals(FrequencyType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
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
