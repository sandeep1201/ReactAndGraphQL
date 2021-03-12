using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRequestForAssistanceRepository
    {
        IRequestForAssistance              GetRfa(string                                      pin, int id);
        ICFRfaDetail                       GetCfRfaDetail(int                                 rfaId);
        ITJTMJRfaDetail                    GetTjTmjRfaDetail(int                              rfaId);
        IFCDPRfaDetail                     GetFcdpRfaDetail(int                               rfaId);
        IEnumerable<IRequestForAssistance> GetRfasForPin(string                               pin);
        IEnumerable<ISP_DB2_RFAs_Result>   GetOldRfasForPin(string                            pin);
        IRequestForAssistance              NewRfa(IParticipant                                participant, string   user);
        ICFRfaDetail                       NewCfRfaDetail(IRequestForAssistance               rfa,         string   user,        DateTime updateDate);
        ITJTMJRfaDetail                    NewTjTmjRfaDetail(IRequestForAssistance            rfa,         string   user,        DateTime updateDate);
        IFCDPRfaDetail                     NewFcdpRfaDetail(IRequestForAssistance             rfa,         string   user,        DateTime updateDate);
        IRequestForAssistanceChild         NewRequestForAssistanceChild(IRequestForAssistance parentRfa,   DateTime date,        string   user);
        bool                               HasAnyRfasInStatus(decimal                         pin,         string   programCode, string[] rfaStatuses);
        bool                               HasAnyActiveProgramRfas(decimal                    pin,         string   programCode);
        DateTime?                          AddBusinessDays(DateTime?                          fromDate,    int      daysForward = 10);
        void                               WriteBackReferralToDb2(IRequestForAssistance       rfa,         DateTime effectiveDate, string userId);
        decimal                            GenerateRFANumberFromDB2(IRequestForAssistance     rfa,         string   userId);
        void                               DenyRFAInDB2(IRequestForAssistance                 rfa,         string   mainframeUserId);
        string                             GetReferralRegCode(decimal?                        pin,         string   programCode);
    }
}
