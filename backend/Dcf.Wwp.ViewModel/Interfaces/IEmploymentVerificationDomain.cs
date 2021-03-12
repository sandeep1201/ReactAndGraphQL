using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IEmploymentVerificationDomain
    {
        List<EmploymentVerificationContract> GetTJTMJEmploymentsForParticipantByJobType(int participantId, int                                  jobTypeId, DateTime enrollmentDate);
        void                                 UpsertEmploymentVerification(string            pin,           List<EmploymentVerificationContract> contract);
    }
}
