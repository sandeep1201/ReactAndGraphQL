using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentInformationJobDutiesDetailsBridge : BaseCommonModel, IEmploymentInformationJobDutiesDetailsBridge, IEquatable<EmploymentInformationJobDutiesDetailsBridge>
    {
        IJobDutiesDetail IEmploymentInformationJobDutiesDetailsBridge.JobDutiesDetail
        {
            get { return JobDutiesDetail; }
            set { JobDutiesDetail = (JobDutiesDetail)value; }
        }

        IEmploymentInformation IEmploymentInformationJobDutiesDetailsBridge.EmploymentInformation
        {
            get { return EmploymentInformation; }
            set { EmploymentInformation = (EmploymentInformation) value; }
        }

        #region ICloneable

        public new object Clone()
        {
            var eidb = new EmploymentInformationJobDutiesDetailsBridge();

            eidb.Id                      = this.Id;
            eidb.EmploymentInformationId = this.EmploymentInformationId;
            eidb.JobDutiesId             = this.JobDutiesId;
            eidb.JobDutiesDetail         = (JobDutiesDetail) this.JobDutiesDetail.Clone();
            eidb.IsDeleted               = this.IsDeleted;
            eidb.ModifiedDate            = this.ModifiedDate;
            eidb.ModifiedBy              = this.ModifiedBy;

            return eidb;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EmploymentInformationJobDutiesDetailsBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(EmploymentInformationJobDutiesDetailsBridge other)
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
            if (!AdvEqual(JobDutiesId, other.JobDutiesId))
                return false;
            if (!AdvEqual(JobDutiesDetail, other.JobDutiesDetail))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(ModifiedBy, other.ModifiedBy))
                return false;
            if (!AdvEqual(ModifiedDate, other.ModifiedDate))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
