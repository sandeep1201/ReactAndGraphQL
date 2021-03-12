using System;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class YesNoSkipLookup : IYesNoSkipLookup, IEquatable<YesNoSkipLookup>
    {
        #region ICloneable

        public object Clone()
        {
            var obj = new YesNoSkipLookup
                      {
                          Id   = this.Id,
                          Name = this.Name
                      };

            return obj;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as YesNoSkipLookup;
            return obj != null && Equals(obj);
        }

        public bool Equals(YesNoSkipLookup other)
        {
            //Check whether the compared object is null.
            if (object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
