using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class Transaction : BaseCommonModel, ITransaction, IEquatable<Transaction>
    {
        #region ICloneable

        public object Clone()
        {
            var clone = new Transaction
                        {
                            Id                = Id,
                            ParticipantId     = ParticipantId,
                            WorkerId          = WorkerId,
                            OfficeId          = OfficeId,
                            TransactionTypeId = TransactionTypeId,
                            Description       = Description,
                            EffectiveDate     = EffectiveDate,
                            CreatedDate       = CreatedDate,
                            IsDeleted         = IsDeleted
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

            var obj = other as Transaction;

            return obj != null && Equals(obj);
        }

        public bool Equals(Transaction other)
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

            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(ParticipantId, other.ParticipantId))
            {
                return false;
            }

            if (!AdvEqual(WorkerId, other.WorkerId))
            {
                return false;
            }

            if (!AdvEqual(OfficeId, other.OfficeId))
            {
                return false;
            }

            if (!AdvEqual(TransactionTypeId, other.TransactionTypeId))
            {
                return false;
            }

            if (!AdvEqual(Description, other.Description))
            {
                return false;
            }

            if (!AdvEqual(EffectiveDate, other.EffectiveDate))
            {
                return false;
            }

            if (!AdvEqual(CreatedDate, other.CreatedDate))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
