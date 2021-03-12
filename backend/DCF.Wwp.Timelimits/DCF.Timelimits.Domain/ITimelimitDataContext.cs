using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DCF.Core.Domain
{
    public interface ITimelimitDataContext
    {
        DbSet<ApprovalReason> ApprovalReasons { get; set; }
        DbSet<AuxiliaryPayment> AuxiliaryPayments { get; set; }
        DbSet<ChangeReason> ChangeReasons { get; set; }
        DbSet<DeleteReason> DeleteReasons { get; set; }
        DbSet<Participant> Participants { get; set; }
        DbSet<TimeLimit> TimeLimits { get; set; }

        Task<Int32> InsertDb2T0459InW2LimitsAsync(String pinNum, String benefitMm, String historySeqNum, String clockTypeCd, String creTranCd, String fedClockInd, String fedCmpMthNum, String fedMaxMthNum, String historyCd, String otCmpMthNum, String overrideReasonCd, String totCmpMthNum, String totMaxMthNum, String updatedDt, String userId, String wwCmpMthNum, String wwMaxMthNum, String commentTxt, CancellationToken token = default(CancellationToken));
        List<AuxiliaryPayment> SpAuxiliaryPayment(String pinNumber);
        Task<List<AuxiliaryPayment>> SpAuxiliaryPaymentAsync(String pinNumber, CancellationToken token = default(CancellationToken));
        List<TimelimitDataContext.SpClockPlacementSummaryReturnModel> SpClockPlacementSummary(String pinNumber);
        Task<List<TimelimitDataContext.SpClockPlacementSummaryReturnModel>> SpClockPlacementSummaryAsync(String pinNumber, CancellationToken token = default(CancellationToken));
        List<SpConfidentialCaseReturnModel> SpConfidentialCase(String caseNumber);
        Task<List<SpConfidentialCaseReturnModel>> SpConfidentialCaseAsync(String caseNumber, CancellationToken token = default(CancellationToken));
        List<SpOtherParticipantReturnModel> SpOtherParticipant(String pinNumber);
        Task<List<SpOtherParticipantReturnModel>> SpOtherParticipantAsync(String pinNumber, CancellationToken token = default(CancellationToken));
        List<SpTimeLimitParticipantReturnModel> SpTimeLimitParticipant(String pinNumber);
        Task<List<SpTimeLimitParticipantReturnModel>> SpTimeLimitParticipantAsync(String pinNumber, CancellationToken token = default(CancellationToken));
        Task<Int32> UpdateDb2T0459InW2LimitsAsync(String pinNum, String benefitMm, String historySeqNum, String clockTypeCd, String creTranCd, String fedClockInd, String fedCmpMthNum, String fedMaxMthNum, String historyCd, String otCmpMthNum, String overrideReasonCd, String totCmpMthNum, String totMaxMthNum, String updatedDt, String userId, String wwCmpMthNum, String wwMaxMthNum, String commentTxt, CancellationToken token = default(CancellationToken));
    }
}