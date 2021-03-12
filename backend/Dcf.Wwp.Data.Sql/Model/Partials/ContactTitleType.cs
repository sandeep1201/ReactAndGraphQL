using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof (ModelExtension))]
    public partial class ContactTitleType : BaseEntity, IContactTitleType, IEquatable<ContactTitleType>
    {
        ICollection<IContact> IContactTitleType.Contacts
        {
            get { return Contacts.Cast<IContact>().ToList(); }
            set { Contacts = value.Cast<Contact>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var ctt = new ContactTitleType();
            ctt.Id        = this.Id;
            ctt.SortOrder = this.SortOrder;
            ctt.Name      = this.Name;
            return ctt;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ContactTitleType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ContactTitleType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)               &&
                   SortOrder.Equals(other.SortOrder) &&
                   Name.Equals(other.Name);
        }

        #endregion IEquatable<T>
    }
}
