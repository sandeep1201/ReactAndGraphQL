using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SupportiveServiceType : BaseCommonModel, ISupportiveServiceType, IEquatable<SupportiveServiceType>
    {
        ICollection<ISupportiveService> ISupportiveServiceType.SupportiveServices
        {
            get { return SupportiveServices.Cast<ISupportiveService>().ToList(); }
            set { SupportiveServices = (ICollection<SupportiveService>) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var sst = new SupportiveServiceType();

            sst.Id        = this.Id;
            sst.Name      = this.Name;
            sst.SortOrder = this.SortOrder;
            sst.IsDeleted = this.IsDeleted;
            return sst;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as SupportiveServiceType;
            return obj != null && Equals(obj);
        }

        public bool Equals(SupportiveServiceType other)
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
