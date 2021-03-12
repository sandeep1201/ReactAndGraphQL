using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ExamResult : BaseCommonModel, IExamResult, IEquatable<ExamResult>
    {
        [NotMapped]
        public float? ScorePercentage
        {
            get
            {
                if (Score.HasValue && MaxScoreRange.HasValue)
                    return 100f * ((float) Score.Value) / ((float) MaxScoreRange.Value);

                return null;
            }
            set { }
        }

        INRSType IExamResult.NRSType
        {
            get { return NRSType; }
            set { NRSType = (NRSType) value; }
        }

        ISPLType IExamResult.SPLType
        {
            get { return SPLType; }
            set { SPLType = (SPLType) value; }
        }

        IExamSubjectType IExamResult.ExamSubjectType
        {
            get { return ExamSubjectType; }
            set { ExamSubjectType = (ExamSubjectType) value; }
        }

        IEducationExam IExamResult.EducationExam
        {
            get { return EducationExam; }
            set { EducationExam = (EducationExam) value; }
        }

        IExamPassType IExamResult.ExamPassType
        {
            get { return ExamPassType; }
            set { ExamPassType = (ExamPassType) value; }
        }

        IExamEquivalencyType IExamResult.ExamEquivalencyType
        {
            get { return ExamEquivalencyType; }
            set { ExamEquivalencyType = (ExamEquivalencyType) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var er = new ExamResult();
            er.Id                = this.Id;
            er.EducationExamId   = this.EducationExamId;
            er.ExamSubjectTypeId = this.ExamSubjectTypeId;
            er.NRSTypeId         = this.NRSTypeId;
            er.SPLTypeId         = this.SPLTypeId;
            //er.VersionTypeId = this.VersionTypeId;
            er.Version               = this.Version;
            er.Score                 = this.Score;
            er.ExamLevelType         = this.ExamLevelType;
            er.ExamPassTypeId        = this.ExamPassTypeId;
            er.DatePassed            = this.DatePassed;
            er.MaxScoreRange         = this.MaxScoreRange;
            er.ExamEquivalencyTypeId = this.ExamEquivalencyTypeId;
            er.GradeEquivalency      = this.GradeEquivalency;
            er.ExamPassType          = (ExamPassType) this.ExamPassType?.Clone();
            er.ExamSubjectType       = (ExamSubjectType) this.ExamSubjectType?.Clone();
            er.NRSType               = (NRSType) this.NRSType?.Clone();
            er.SPLType               = (SPLType) this.SPLType?.Clone();
            er.ExamEquivalencyType   = (ExamEquivalencyType) this.ExamEquivalencyType?.Clone();
            return er;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ExamResult;
            return obj != null && Equals(obj);
        }

        public bool Equals(ExamResult other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EducationExamId, other.EducationExamId))
                return false;
            if (!AdvEqual(ExamSubjectTypeId, other.ExamSubjectTypeId))
                return false;
            if (!AdvEqual(NRSTypeId, other.NRSTypeId))
                return false;
            if (!AdvEqual(SPLTypeId, other.SPLTypeId))
                return false;
            if (!AdvEqual(DatePassed, other.DatePassed))
                return false;
            if (!AdvEqual(MaxScoreRange, other.MaxScoreRange))
                return false;
            if (!AdvEqual(Level, other.Level))
                return false;
            if (!AdvEqual(Form, other.Form))
                return false;
            if (!AdvEqual(CasasGradeEquivalency, other.CasasGradeEquivalency))
                return false;
            if (!AdvEqual(Version, other.Version))
                return false;
            if (!AdvEqual(Score, other.Score))
                return false;
            if (!AdvEqual(ExamPassTypeId, other.ExamPassTypeId))
                return false;
            if (!AdvEqual(ExamLevelType, other.ExamLevelType))
                return false;
            if (!AdvEqual(ExamEquivalencyTypeId, other.ExamEquivalencyTypeId))
                return false;
            if (!AdvEqual(GradeEquivalency, other.GradeEquivalency))
                return false;
            if (!AdvEqual(NRSType, other.NRSType))
                return false;
            if (!AdvEqual(SPLType, other.SPLType))
                return false;
            if (!AdvEqual(ExamSubjectType, other.ExamSubjectType))
                return false;
            if (!AdvEqual(ExamPassType, other.ExamPassType))
                return false;
            if (!AdvEqual(ExamEquivalencyType, other.ExamEquivalencyType))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
