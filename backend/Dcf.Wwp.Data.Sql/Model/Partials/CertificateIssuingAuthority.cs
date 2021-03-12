using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class CertificateIssuingAuthority : BaseCommonModel, ICertificateIssuingAuthority, IEquatable<CertificateIssuingAuthority>
    {
        #region ICloneable

        public object Clone()
        {
            var ci = new CertificateIssuingAuthority
                     {
                         Id        = Id,
                         SortOrder = SortOrder,
                         Name      = Name,
                         Code      = Code
                     };

            return ci;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as CertificateIssuingAuthority;

            return obj != null && Equals(obj);
        }

        public bool Equals(CertificateIssuingAuthority other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)     &&
                   Name.Equals(other.Name) &&
                   Code.Equals(other.Code) &&
                   SortOrder.Equals(other.SortOrder);
        }

        #endregion IEquatable<T>
    }
}
