using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class OtherJobInformation : BaseCommonModel, IOtherJobInformation, IEquatable<OtherJobInformation>
    {
        ICollection<IEmploymentInformation> IOtherJobInformation.EmploymentInformations
        {
            get => EmploymentInformations.Cast<IEmploymentInformation>().ToList();
            set => EmploymentInformations = value.Cast<EmploymentInformation>().ToList();
        }

        IJobFoundMethod IOtherJobInformation.JobFoundMethod
        {
            get => JobFoundMethod;
            set => JobFoundMethod = (JobFoundMethod) value;
        }

        IJobSector IOtherJobInformation.JobSector
        {
            get => JobSector;
            set => JobSector = (JobSector) value;
        }

        IWorkProgram IOtherJobInformation.WorkProgram
        {
            get => WorkProgram;
            set => WorkProgram = (WorkProgram) value;
        }

        #region ICloneable

        public object Clone()
        {
            var oji = new OtherJobInformation();

            oji.Id                      = Id;
            oji.ExpectedScheduleDetails = ExpectedScheduleDetails;
            oji.JobSectorId             = JobSectorId;
            oji.JobFoundMethodId        = JobFoundMethodId;
            oji.WorkerId                = WorkerId;
            oji.JobFoundMethodDetails   = JobFoundMethodDetails;
            oji.WorkProgramId           = WorkProgramId;
            oji.JobFoundMethod          = (JobFoundMethod) JobFoundMethod?.Clone();
            oji.JobSector               = (JobSector) JobSector?.Clone();
            oji.WorkProgram             = (WorkProgram) WorkProgram?.Clone();


            return oji;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as OtherJobInformation;
            return obj != null && Equals(obj);
        }

        public bool Equals(OtherJobInformation other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ExpectedScheduleDetails, other.ExpectedScheduleDetails))
                return false;
            if (!AdvEqual(JobSectorId, other.JobSectorId))
                return false;
            if (!AdvEqual(JobFoundMethodId, other.JobFoundMethodId))
                return false;
            if (!AdvEqual(WorkerId, other.WorkerId))
                return false;
            if (!AdvEqual(JobFoundMethodDetails, other.JobFoundMethodDetails))
                return false;
            if (!AdvEqual(WorkProgramId, other.WorkProgramId))
                return false;
            if (!AdvEqual(JobFoundMethod, other.JobFoundMethod))
                return false;
            if (!AdvEqual(JobSector, other.JobSector))
                return false;
            if (!AdvEqual(WorkProgram, other.WorkProgram))
                return false;

            // Are Equal.
            return true;
        }

        #endregion IEquatable<T>
    }
}
