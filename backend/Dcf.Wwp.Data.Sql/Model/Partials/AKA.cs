using System;
using System.ComponentModel.DataAnnotations;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class AKA : BaseCommonModel, IAKA, IEquatable<AKA>
    {
        IParticipant IAKA.Participant
        {
            get => Participant;
            set => Participant = (Participant) value;
        }

        #region ICloneable

        public object Clone()
        {
            return new AKA
                   {
                       Id            = Id,
                       ParticipantId = ParticipantId,
                       SSNNumber     = SSNNumber,
                       SSNTypeId     = SSNTypeId,
                       Details       = Details,
                       IsDeleted     = IsDeleted
                   };
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as AKA;
            return obj != null && Equals(obj);
        }

        public bool Equals(AKA other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(SSNNumber, other.SSNNumber))
                return false;
            if (!AdvEqual(SSNTypeId, other.SSNTypeId))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
