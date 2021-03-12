using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityContactBridge : BaseCommonModel, IActivityContactBridge, IEquatable<ActivityContactBridge>
    {
        IActivity IActivityContactBridge.Activity
        {
            get => Activity;
            set => Activity = (Activity) value;
        }

        IContact IActivityContactBridge.Contact
        {
            get => Contact;
            set => Contact = (Contact) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new ActivityContactBridge
                        {
                            ActivityId = ActivityId,
                            ContactId  = ContactId
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as ActivityContactBridge;

            return obj != null && Equals(obj);
        }

        public bool Equals(ActivityContactBridge other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check whether the products' properties are equal.           
            if (!AdvEqual(ActivityId, other.ActivityId))
            {
                return false;
            }

            if (!AdvEqual(ContactId, other.ContactId))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
