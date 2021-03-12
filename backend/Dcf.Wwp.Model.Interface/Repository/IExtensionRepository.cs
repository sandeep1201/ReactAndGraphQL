using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IExtensionRepository
    {
        IEnumerable<ITimeLimitExtension> GetExtensionSequenceExtensionsByExtById(Int32 id);
        IEnumerable<ITimeLimitExtension> GetExtensionsByPin(String                     pin);
        ITimeLimitExtension              GetExtensionsById(Int32                       id);
        ITimeLimitExtension              GetCurrentExtensionByType(Int32               timelimitTypeId, String pin);
        IEnumerable<IApprovalReason>     GetExtensionApprovalReasons();
        IEnumerable<IDenialReason>       GetExtensionDenialReasons();
        ITimeLimitExtension              NewTimeLimitExtension();

        ITimeLimitExtension    GetExensionByDateRange(Int32 participantId, Int32 timelimitTypeId, DateTime startDate, DateTime endDate);
        string                 GetTimeLimitType(int?        timelimitTypeId);
        string                 GetExtensionDecision(int?    extensionDecisionId);
        IQueryable<ITimeLimit> GetTimeLimit(int             participantId);
    }
}
