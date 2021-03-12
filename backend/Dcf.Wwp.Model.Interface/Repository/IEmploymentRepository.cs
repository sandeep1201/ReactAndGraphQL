using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEmploymentRepository
    {
        IEmploymentInformation                        EmploymentByIdAsNoTracking(int?                          employmentId);
        IEmploymentInformation                        EmploymentById(Int32?                                    employmentId);
        IEmploymentInformation                        NewEmploymentInfo(IParticipant                           participant,   String user);
        void                                          SP_Work_History_WriteBack(int?                      participantId, short? eSeqNo, string mFUserId, bool isDeletedEmployment, bool isNewEmployment, string computedDB2WageRateValue);
        void                                          DeleteOnFailure(IEmploymentInformation                   employmentInformation);
        bool                                          EmploymentInfoTransactionalSave(IEmploymentInformation   employmentInformation, string user,     string      mFUserId, string computedDB2WageRateUnit, string computedDB2WageRateValue);
        void                                          EmploymentInfoTransactionalDelete(IEmploymentInformation employmentInfo,        string mFUserId, IEPEIBridge epei);
        ISP_DB2_PreCheck_POP_Claim_Result             PreCheckPop(string                                       pin,                   short? seqNo);
        IEnumerable<IUSP_ProgramStatus_Recent_Result> GetRecentPEPForPin(decimal?                              pin);
    }
}
