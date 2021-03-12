using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EARequestParticipantBridge : BaseCommonModel, IEARequestParticipantBridge, IEquatable<EARequestParticipantBridge>
    {
        IParticipant IEARequestParticipantBridge.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new EARequestParticipantBridge
                        {
                            Id            = Id,
                            ParticipantId = ParticipantId,
                            EARequestId   = EARequestId
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

            var obj = other as EARequestParticipantBridge;

            return obj != null && Equals(obj);
        }

        public bool Equals(EARequestParticipantBridge other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
                return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
                return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;

            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;

            if (!AdvEqual(EARequestId, other.EARequestId))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
