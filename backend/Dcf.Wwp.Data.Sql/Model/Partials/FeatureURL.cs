using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FeatureURL : BaseCommonModel, IFeatureURL
    {
        #region ICloneable

        public new object Clone()
        {
            var fu = new FeatureURL();

            fu.Id        = this.Id;
            fu.Feature   = this.Feature;
            fu.URL       = this.URL;
            fu.IsDeleted = this.IsDeleted;
            return fu;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as FeatureURL;
            return obj != null && Equals(obj);
        }

        public bool Equals(FeatureURL other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Feature, other.Feature))
                return false;
            if (!AdvEqual(URL, other.URL))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
