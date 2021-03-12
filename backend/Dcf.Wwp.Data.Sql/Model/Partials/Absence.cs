using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Absence : BaseCommonModel, IAbsence, IEquatable<Absence>
    {
        IAbsenceReason IAbsence.AbsenceReason
        {
            get { return AbsenceReason; }
            set { AbsenceReason = (AbsenceReason) value; }
        }

        IEmploymentInformation IAbsence.EmploymentInformation
        {
            get { return EmploymentInformation; }
            set { EmploymentInformation = (EmploymentInformation) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var ab = new Absence();
            ab.Id                      = this.Id;
            ab.EmploymentInformationId = this.EmploymentInformationId;
            ab.BeginDate               = this.BeginDate;
            ab.EndDate                 = this.EndDate;
            ab.AbsenceReasonId         = this.AbsenceReasonId;
            ab.Details                 = this.Details;
            ab.IsDeleted               = this.IsDeleted;
            ab.AbsenceReason           = (AbsenceReason) this.AbsenceReason?.Clone();
            return ab;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            var obj = other as Absence;
            return obj != null && Equals(obj);
        }

        public bool Equals(Absence other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmploymentInformationId, other.EmploymentInformationId))
                return false;
            if (!AdvEqual(BeginDate, other.BeginDate))
                return false;
            if (!AdvEqual(EndDate, other.EndDate))
                return false;
            if (!AdvEqual(AbsenceReasonId, other.AbsenceReasonId))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            if (!AdvEqual(AbsenceReason, other.AbsenceReason))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
