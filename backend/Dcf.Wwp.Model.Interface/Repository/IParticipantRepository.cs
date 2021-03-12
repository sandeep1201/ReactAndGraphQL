using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IParticipantRepository
    {
        IParticipant                                   GetParticipant(string          pin);
        IParticipant                                   GetRefreshedParticipant(string pin);
        ISP_ParticipantDetailsReturnType               GetParticipantDetails(string   pin);
        IEnumerable<IParticipant>                      GetAllParticipants();
        IParticipant                                   GetParticipantById(int?                          id);
        IParticipant                                   GetParticipantByMciId(decimal                    mciId);
        IEnumerable<IParticipant>                      GetRecentParticipantsByUser(string               userId, int limit);
        IEnumerable<IParticipantEnrolledProgram>       GetNonEligibilityReferrals(IWorker               worker);
        IParticipantEnrolledProgram                    GetParticantEnrollment(int                       pepId);
        ISP_PreCheckDisenrollment_Result               PreDisenrollmentErrors(decimal?                  pinNumer, decimal? caseNumber,    int?   pepId);
        void                                           UpdateDisenrollment(IParticipantEnrolledProgram  pep,      int?     workerLoginId, string mFUserId, string authWorker, string completionReasonDetails = null, DateTime? disenrollmentDate = null, int? completionReasonId = null);
        IEnumerable<IParticipant>                      PariticipantsBeingTransferred(IWorker            worker);
        IEnumerable<IParticipantEnrolledProgram>       GetPepRecordsForPin(decimal                      pin);
        IEnumerable<IUSP_ReferralsAndTransfers_Result> GetReferralsAndTransfersResults(IWorker          worker, bool refreshInd, string agencyCode, string roles);
        IEnumerable<IUSP_GetLastWWOrLFInstance>        GetLastWWOrLFInstance(decimal                    pin);
        string                                         GetEnrolledProgramStatus(int?                    enrolledProgStatusId);
        string                                         GetEnrolledProgramCd(string                      enrolledProgramName);
        void                                           EnrollPep(int?                                   pepId,     int?    workerLoginId, string  userId);
        void                                           TransferPariticipant(IParticipantEnrolledProgram pep,       IOffice sourceOffice,  IOffice destOffice, IWorker sourceWorker, IWorker destWorker, string userId, string t2536Fep);
        void                                           UpsertRecentParticipant(string                   userId,    int     participantId);
        void                                           UpsertParticipantEnrollment(int?                 pepId,     int?    workerLoginId, string action, string userId, string worker, string completionReasonDetails = null, DateTime? disenrollmentDate = null, int? completionReasonId = null);
        void                                           ReassignLFCaseManagerInDB2(decimal?              pinNumber, string  mFUserId);
        void                                           ReassignW2CaseManagerInDB2(decimal?              pinNumber, string  FepId);
        IEnumerable<IConfidentialPinInformation>       GetConfidentialPinInfo(decimal                   pin);
        void                                           UpdateT0532(decimal?                             pin, string mfUserId, string programCode);
        DataTable                                      GetMostRecentPrograms(decimal                    pin);
        IEnumerable<ISpecialInitiative>                GetFeatureValue(string                           featureName);
        IWorkerTaskStatus                              GetWorkerTaskStatus(string                       code);
        IWorkerTaskCategory                            GetWorkerTaskCategory(string                     code);

        #region Performance-related

        IEnumerable<IUSP_RecentlyAccessed_ProgramStatus_Result>    GetRecentParticipants(string    wamsId);
        IEnumerable<IUSP_RecentlyAccessed_ProgramStatus_Result>    GetParticipantsBySearch(string  firstName, string lastName,   string middleName, string gender, DateTime? dob);
        IEnumerable<IUSP_ParticipantbyWorker_ProgramStatus_Result> GetParticipantsForWorker(string wamsId,    string agencyCode, string program);
        IQueryable<IParticipant>                                   GetParticipantsAsQueryable();
        IQueryable<IRecentParticipant>                             GetRecentParticipantsAsQueryable();

        #endregion
    }
}
