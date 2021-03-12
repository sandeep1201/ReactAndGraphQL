using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IParticipantDomain
    {
        #region Properties

        #endregion

        #region Methods

        bool                            IsValidPin(string                                 pin);
        List<ParticipantStatusContract> GetCurrentStatusesForPin(string                   pin);
        ParticipantStatusContract       GetStatus(string                                  pin, int id);
        List<ParticipantStatusContract> GetAllStatusesForPin(string                       pin);
        List<ParticipantStatusContract> GetStatusesForPin(string                          pin, bool allStatuses = true);
        ParticipantStatusContract       AddStatus(ParticipantStatusContract               psc);
        ParticipantStatusContract       UpdateStatus(ParticipantStatusContract            psc);
        List<Activity>                  GetActivitiesForTjOrTmjProgramsForPreCheck(string pin);
        List<Activity>                  GetActivitiesForPreCheckBasedOnProgram(string     pin,        int?     enrolledProgramId);
        int                             ExecSanctionableSP(string                         modifiedBy, DateTime modifiedDate,        int?  participantId = null, DateTime? beginDate = null, DateTime? maxDate = null, IEnumerable<ParticipationEntry> pe = null);
        PaymentDetailsContract          GetDetailsBasedOnParticipationPeriod(string       pin,        string   participationPeriod, short year, string caseNumber);

        List<decimal> GetCaseNumbersBasedOnParticipationPeriod(string pin, string participationPeriod, short year);

        #endregion
    }
}
