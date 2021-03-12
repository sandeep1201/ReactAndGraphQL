using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobType : BaseCommonModel, IJobType, IEquatable<JobType>
    {
        ICollection<IEmploymentInformation> IJobType.EmploymentInformations
        {
            get { return EmploymentInformations.Cast<IEmploymentInformation>().ToList(); }
            set { EmploymentInformations = value.Cast<EmploymentInformation>().ToList(); }
        }

        ICollection<IJobTypeLeavingReasonBridge> IJobType.JobTypeLeavingReasonBridges
        {
            get { return JobTypeLeavingReasonBridges.Cast<IJobTypeLeavingReasonBridge>().ToList(); }
            set { JobTypeLeavingReasonBridges = value.Cast<JobTypeLeavingReasonBridge>().ToList(); }
        }

        #region ICloneable

        public new object Clone()
        {
            var jt = new JobType();

            jt.Id         = this.Id;
            jt.Name       = this.Name;
            jt.IsRequired = this.IsRequired;
            jt.SortOrder  = this.SortOrder;
            jt.Name       = this.Name;
            return jt;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as JobType;
            return obj != null && Equals(obj);
        }

        public bool Equals(JobType other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id)               &&
                   Name.Equals(other.Name)           &&
                   SortOrder.Equals(other.SortOrder) &&
                   IsRequired.Equals(other.IsRequired);
        }

        #endregion IEquatable<T>
    }
}
