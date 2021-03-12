using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IInformalAssessmentRepository
    {
        IInformalAssessment NewInformalAssessment(int  participantId, bool isSubsequent, string user);
        IInformalAssessment InformalAssessmentById(int id);

        void                SP_DB2_InformalAssessment_Update(decimal? pinNumber, string MFWorkerId);
        IInformalAssessment GetMostRecentAssessment(IParticipant      part);
    }
}
