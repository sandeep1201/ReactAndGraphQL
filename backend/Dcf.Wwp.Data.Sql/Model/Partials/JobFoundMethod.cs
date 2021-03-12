using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobFoundMethod : BaseCommonModel, IJobFoundMethod, IEquatable<JobFoundMethod>
    {
        ICollection<IOtherJobInformation> IJobFoundMethod.OtherJobInformations
        {
            get { return OtherJobInformations.Cast<IOtherJobInformation>().ToList(); }
            set { OtherJobInformations = value.Cast<OtherJobInformation>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var jfm = new JobFoundMethod();

            jfm.Id        = this.Id;
            jfm.Name      = this.Name;
            jfm.SortOrder = this.SortOrder;
            jfm.Name      = this.Name;
            return jfm;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as JobFoundMethod;
            return obj != null && Equals(obj);
        }

        public bool Equals(JobFoundMethod other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)     &&
                   Name.Equals(other.Name) &&
                   SortOrder.Equals(other.SortOrder);
        }

        #endregion IEquatable<T>
    }
}
