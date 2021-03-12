using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AbsenceReason : BaseCommonModel, IAbsenceReason, IEquatable<AbsenceReason>
    {
        ICollection<IAbsence> IAbsenceReason.Absences
        {
            get { return Absences.Cast<IAbsence>().ToList(); }
            set { Absences = (ICollection<Absence>)value; }
        }

        #region ICloneable

        public object Clone()
        {
            var ab = new AbsenceReason();

            ab.Id        = Id;
            ab.SortOrder = SortOrder;
            ab.Name      = Name;

            return ab;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as AbsenceReason;

            return obj != null && Equals(obj);
        }

        public bool Equals(AbsenceReason other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(SortOrder, other.SortOrder))
            {
                return false;
            }

            if (!AdvEqual(Name, other.Name))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
