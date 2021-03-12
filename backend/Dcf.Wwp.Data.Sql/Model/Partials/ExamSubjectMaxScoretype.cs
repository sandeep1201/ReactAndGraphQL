using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ExamSubjectMaxScoreType : IExamSubjectMaxScoreType, IEquatable<ExamSubjectMaxScoreType>
    {
        IExamType IExamSubjectMaxScoreType.ExamType
        {
            get { return ExamType; }
            set { ExamType = (ExamType) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var emxs = new ExamSubjectMaxScoreType();
            emxs.Id                = this.Id;
            emxs.ExamTypeId        = this.ExamTypeId;
            emxs.ExamSubjectTypeId = this.ExamSubjectTypeId;
            emxs.MaxScore          = this.MaxScore;
            //emxs.ExamSubjectType = (ExamSubjectType)this.ExamSubjectType?.Clone();
            emxs.ExamType = (ExamType) this.ExamType?.Clone();
            return emxs;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ExamSubjectMaxScoreType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ExamSubjectMaxScoreType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ExamTypeId, other.ExamTypeId))
                return false;
            if (!AdvEqual(ExamSubjectTypeId, other.ExamSubjectTypeId))
                return false;
            if (!AdvEqual(MaxScore, other.MaxScore))
                return false;
            if (!AdvEqual(ExamType, other.ExamType))
                return false;
            //if (!AdvEqual(ExamSubjectType, other.ExamSubjectType))
            //    return false;
            return true;
        }

        private bool AdvEqual(object obj1, object obj2)
        {
            if (obj1 == null ^ obj2 == null)
            {
                return false;
            }

            if (obj1 == null && obj2 == null)
            {
                return true;
            }

            if (obj1.Equals(obj2))
            {
                return true;
            }

            return false;
        }

        #endregion IEquatable<T>
    }
}
