using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentProgramType : BaseCommonModel, IEmploymentProgramType, IEquatable<EmploymentProgramType>
    {
        ICollection<IEmploymentInformation> IEmploymentProgramType.EmploymentInformations
        {
            get { return EmploymentInformations.Cast<IEmploymentInformation>().ToList(); }
            set { EmploymentInformations = value.Cast<EmploymentInformation>().ToList(); }
        }

        #region ICloneable

        public Object Clone()
        {
            var ept = new EmploymentProgramType();

            ept.Id        = this.Id;
            ept.Name      = this.Name;
            ept.SortOrder = this.SortOrder;
            ept.Name      = this.Name;
            return ept;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmploymentProgramType;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmploymentProgramType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)     &&
                   Name.Equals(other.Name) &&
                   SortOrder.Equals(other.SortOrder);
        }

        #endregion IEquatable<T>
    }
}
