using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipationStatu : BaseCommonModel, IParticipationStatu, IEquatable<ParticipationStatu>
    {
        IEnrolledProgram IParticipationStatu.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        IParticipant IParticipationStatu.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IParticipationStatusType IParticipationStatu.ParticipationStatusType
        {
            get { return ParticipationStatusType; }
            set { ParticipationStatusType = (ParticipationStatusType) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var ep = new ParticipationStatu();

            ep.Id                = this.Id;
            ep.ParticipantId     = this.ParticipantId;
            ep.EnrolledProgramId = this.EnrolledProgramId;
            ep.BeginDate         = this.BeginDate;
            ep.EndDate           = this.EndDate;
            ep.IsDeleted         = this.IsDeleted;
            ep.Details           = this.Details;
            ep.EnrolledProgramId = this.EnrolledProgramId;
            ep.IsCurrent         = this.IsCurrent;
            return ep;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ParticipationStatu;
            return obj != null && Equals(obj);
        }

        public bool Equals(ParticipationStatu other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(EnrolledProgramId, other.EnrolledProgramId))
                return false;
            if (!AdvEqual(BeginDate, other.BeginDate))
                return false;
            if (!AdvEqual(EndDate, other.EndDate))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(EnrolledProgramId, other.EnrolledProgramId))
                return false;
            if (!AdvEqual(IsCurrent, other.IsCurrent))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
