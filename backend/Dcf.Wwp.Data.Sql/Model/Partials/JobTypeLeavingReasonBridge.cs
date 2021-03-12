using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobTypeLeavingReasonBridge : BaseCommonModel, IJobTypeLeavingReasonBridge, IEquatable<JobTypeLeavingReasonBridge>
    {
        IJobType IJobTypeLeavingReasonBridge.JobType
        {
            get => JobType;
            set => JobType = (JobType) value;
        }

        ILeavingReason IJobTypeLeavingReasonBridge.LeavingReason
        {
            get =>  LeavingReason;
            set => LeavingReason = (LeavingReason) value;
        }


        #region ICloneable

        public object Clone()
        {
            var clone = new JobTypeLeavingReasonBridge()
                        {
                            Id              = Id,
                            JobTypeId       = JobTypeId,
                            LeavingReasonId = LeavingReasonId,
                            IsDeleted       = IsDeleted,
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as JobTypeLeavingReasonBridge;

            return obj != null && Equals(obj);
        }

        public bool Equals(JobTypeLeavingReasonBridge other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check whether the products' properties are equal.           
            if (!AdvEqual(JobTypeId, other.JobTypeId))
            {
                return false;
            }

            if (!AdvEqual(LeavingReasonId, other.LeavingReasonId))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
