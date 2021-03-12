using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AddressVerificationTypeLookup : BaseCommonModel, IAddressVerificationTypeLookup, IEquatable<AddressVerificationTypeLookup>
    {
        ICollection<IAlternateMailingAddress> IAddressVerificationTypeLookup.AlternateMailingAddresses
        {
            get { return AlternateMailingAddresses.Cast<IAlternateMailingAddress>().ToList(); }
            set { AlternateMailingAddresses = (ICollection<AlternateMailingAddress>)value; }
        }

        ICollection<IParticipantContactInfo> IAddressVerificationTypeLookup.ParticipantContactInfoes
        {
            get { return ParticipantContactInfoes.Cast<IParticipantContactInfo>().ToList(); }
            set { ParticipantContactInfoes = (ICollection<ParticipantContactInfo>)value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var a = new AddressVerificationTypeLookup();

            a.Id = this.Id;
            a.Name = this.Name;
            a.IsDeleted = this.IsDeleted;
            return a;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as AddressVerificationTypeLookup;
            return obj != null && Equals(obj);
        }

        public bool Equals(AddressVerificationTypeLookup other)
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
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
