using System;
using Dcf.Wwp.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class Language : BaseCommonModel, ILanguage, IEquatable<Language>
    {
        #region ICloneable

        public new object Clone()
        {
            var l = new Language();

            l.Id   = this.Id;
            l.Code = this.Code;
            l.Name = this.Name;

            return l;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as Language;
            return obj != null && Equals(obj);
        }

        public bool Equals(Language other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;
            if (!AdvEqual(Code, other.Code))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
