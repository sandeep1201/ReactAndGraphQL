using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantEnrolledProgramCutOverBridge : BaseEntity, IParticipantEnrolledProgramCutOverBridge, IEquatable<ParticipantEnrolledProgramCutOverBridge>
    {
        IParticipant IParticipantEnrolledProgramCutOverBridge.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IEnrolledProgram IParticipantEnrolledProgramCutOverBridge.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var bridge = new ParticipantEnrolledProgramCutOverBridge
                         {
                             Id                = Id,
                             IsDeleted         = IsDeleted,
                             ModifiedBy        = ModifiedBy,
                             ModifiedDate      = ModifiedDate,
                             RowVersion        = RowVersion,
                             ParticipantId     = ParticipantId,
                             EnrolledProgramId = EnrolledProgramId,
                             CutOverDate       = CutOverDate
                         };

            return bridge;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            var obj = other as ParticipantEnrolledProgramCutOverBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(ParticipantEnrolledProgramCutOverBridge other)
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
            if (!AdvEqual(EnrolledProgramId, other.EnrolledProgramId))
                return false;
            if (!AdvEqual(CutOverDate, other.CutOverDate))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(ModifiedBy, other.ModifiedBy))
                return false;
            if (!AdvEqual(ModifiedDate, other.ModifiedDate))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
