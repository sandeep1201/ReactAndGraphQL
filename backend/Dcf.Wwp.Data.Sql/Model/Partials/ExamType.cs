using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ExamType : BaseCommonModel, IExamType, IEquatable<ExamType>
    {
        ICollection<IEducationExam> IExamType.EducationExams
        {
            get { return EducationExams.Cast<IEducationExam>().ToList(); }

            set { EducationExams = value.Cast<EducationExam>().ToList(); }
        }

        ICollection<IExamSubjectMaxScoreType> IExamType.ExamSubjectMaxScoreTypes
        {
            get { return ExamSubjectMaxScoreTypes.Cast<IExamSubjectMaxScoreType>().ToList(); }

            set { ExamSubjectMaxScoreTypes = value.Cast<ExamSubjectMaxScoreType>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var et = new ExamType();
            et.Id        = this.Id;
            et.SortOrder = this.SortOrder;
            et.Name      = this.Name;
            return et;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ExamType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ExamType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(Name, other.Name))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
