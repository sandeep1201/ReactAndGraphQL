using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class EducationExam : BaseCommonModel, IEducationExam, IEquatable<EducationExam>
    {
        ICollection<IExamResult> IEducationExam.ExamResults
        {
            get { return ExamResults.Cast<IExamResult>().ToList(); }

            set { ExamResults = value.Cast<ExamResult>().ToList(); }
        }

        IExamType IEducationExam.ExamType
        {
            get { return ExamType; }
            set { ExamType = (ExamType) value; }
        }

        IParticipant IEducationExam.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var ee = new EducationExam();
            ee.Id            = this.Id;
            ee.ParticipantId = this.ParticipantId;
            ee.ExamTypeId    = this.ExamTypeId;
            ee.DateTaken     = this.DateTaken;
            ee.Details       = this.Details;
            ee.ExamType      = (ExamType) this.ExamType?.Clone();
            ee.ExamResults   = this.ExamResults.Select(x => (ExamResult) x.Clone()).ToList();
            ee.IsDeleted     = this.IsDeleted;

            return ee;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EducationExam;
            return obj != null && Equals(obj);
        }

        public bool Equals(EducationExam other)
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
            if (!AdvEqual(ExamTypeId, other.ExamTypeId))
                return false;
            if (!AdvEqual(DateTaken, other.DateTaken))
                return false;
            if (!AdvEqual(ExamType, other.ExamType))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (AreBothNotNull(ExamResults, other.ExamResults) && !(ExamResults.OrderBy(x => x.Id).SequenceEqual(other.ExamResults.OrderBy(x => x.Id))))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
