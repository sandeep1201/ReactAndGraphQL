using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class YesNoUnknownLookup : IYesNoUnknownLookup, IEquatable<YesNoUnknownLookup>
    {
        #region ICloneable

        public object Clone()
        {
            var obj = new YesNoUnknownLookup
                      {
                          Id   = Id,
                          Code = Code,
                          Name = Name
                      };

            return obj;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as YesNoUnknownLookup;
            return obj != null && Equals(obj);
        }

        public bool Equals(YesNoUnknownLookup other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)     &&
                   Code.Equals(other.Code) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
