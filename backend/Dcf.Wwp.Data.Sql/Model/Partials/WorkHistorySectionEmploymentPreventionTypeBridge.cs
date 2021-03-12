using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkHistorySectionEmploymentPreventionTypeBridge : BaseCommonModel, IWorkHistorySectionEmploymentPreventionTypeBridge, IEquatable<WorkHistorySectionEmploymentPreventionTypeBridge>
    {
        IEmploymentPreventionType IWorkHistorySectionEmploymentPreventionTypeBridge.EmploymentPreventionType
        {
            get => EmploymentPreventionType;
            set => EmploymentPreventionType = (EmploymentPreventionType) value;
        }

        IWorkHistorySection IWorkHistorySectionEmploymentPreventionTypeBridge.WorkHistorySection
        {
            get => WorkHistorySection;
            set => WorkHistorySection = (WorkHistorySection) value;
        }

        #region ICloneable

        public new object Clone()
        {
            var clone = new WorkHistorySectionEmploymentPreventionTypeBridge();

            clone.Id                         = Id;
            clone.EmploymentPreventionTypeId = EmploymentPreventionTypeId;
            clone.WorkHistorySectionId       = WorkHistorySectionId;
            clone.IsDeleted                  = IsDeleted;

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as WorkHistorySectionEmploymentPreventionTypeBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(WorkHistorySectionEmploymentPreventionTypeBridge other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(EmploymentPreventionTypeId, other.EmploymentPreventionTypeId))
                return false;
            if (!AdvEqual(WorkHistorySectionId, other.WorkHistorySectionId))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
