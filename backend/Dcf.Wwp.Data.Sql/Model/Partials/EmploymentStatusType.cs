using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class EmploymentStatusType : BaseCommonModel, IEmploymentStatusType, IEquatable<EmploymentStatusType>
    {
        ICollection<IWorkHistorySection> IEmploymentStatusType.WorkHistorySections
        {
            get { return WorkHistorySections.Cast<IWorkHistorySection>().ToList(); }
            set { WorkHistorySections = value.Cast<WorkHistorySection>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var es = new EmploymentStatusType();

            es.Id        = this.Id;
            es.SortOrder = this.SortOrder;
            es.Name      = this.Name;
            return es;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmploymentStatusType;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmploymentStatusType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (AreBothNotNull(Id, other.Id) && (!Id.Equals(other.Id)) || EitherNotNull(Id, other.Id) && (!Id.Equals(other.Id)))
                return false;
            if (AreBothNotNull(SortOrder, other.SortOrder) && (!SortOrder.Equals(other.SortOrder)) || EitherNotNull(SortOrder, other.SortOrder) && (!SortOrder.Equals(other.SortOrder)))
                return false;
            if (AreBothNotNull(Name, other.Name) && (!Name.Equals(other.Name)) || EitherNotNull(Name, other.Name) && (!Name.Equals(other.Name)))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
