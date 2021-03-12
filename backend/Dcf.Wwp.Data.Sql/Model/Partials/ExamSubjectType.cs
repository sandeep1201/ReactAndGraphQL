using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ExamSubjectType : BaseEntity, IExamSubjectType, IEquatable<ExamSubjectType>
    {
        ICollection<IExamResult> IExamSubjectType.ExamResults
        {
            get { return ExamResults.Cast<IExamResult>().ToList(); }
            set { ExamResults = value.Cast<ExamResult>().ToList(); }
        }

        IExamType IExamSubjectType.ExamType
        {
            get { return ExamType; }
            set { ExamType = (ExamType) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var et = new ExamSubjectType();
            et.Id         = this.Id;
            et.SortOrder  = this.SortOrder;
            et.Name       = this.Name;
            et.ExamTypeId = this.ExamTypeId;
            et.ExamType   = this.ExamType;

            return et;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as ExamSubjectType;
            return obj != null && Equals(obj);
        }

        public bool Equals(ExamSubjectType other)
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
            if (!AdvEqual(ExamTypeId, other.ExamTypeId))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
