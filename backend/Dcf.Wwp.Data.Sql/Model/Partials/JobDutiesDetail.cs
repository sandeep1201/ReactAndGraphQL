using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobDutiesDetail : BaseCommonModel, IJobDutiesDetail, IEquatable<JobDutiesDetail>
    {
        ICollection<IEmploymentInformationJobDutiesDetailsBridge> IJobDutiesDetail.EmploymentInformationJobDutiesDetailsBridges
        {
            get { return EmploymentInformationJobDutiesDetailsBridges.Cast<IEmploymentInformationJobDutiesDetailsBridge>().ToList(); }
            set { EmploymentInformationJobDutiesDetailsBridges = value.Cast<EmploymentInformationJobDutiesDetailsBridge>().ToList(); }
        }

        #region ICloneable

        public object Clone()
        {
            var jd = new JobDutiesDetail();

            jd.Id      = this.Id;
            jd.Details = this.Details;
            return jd;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as JobDutiesDetail;
            return obj != null && Equals(obj);
        }

        public bool Equals(JobDutiesDetail other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(Details, other.Details))
                return false;
            return true;
        }

        #endregion IEquatable<T>
    }
}
