using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Configuration;
using DCF.Common.Logging;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using EnumsNET;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ExtensionSequenceViewModel : BasePinViewModel
    {
        private readonly ITimelimitService _timelimitService;
        private readonly IDb2TimelimitService _db2TimelimitService;

        public ExtensionSequenceViewModel(IRepository repository, IAuthUser authUser, ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService) : base(repository, authUser)
        {
            this._timelimitService = timelimitService;
            this._db2TimelimitService = db2TimelimitService;
        }


        public ExtensionSequenceContract GetExtensionSequenceById(Int32 extensionId)
        {
            ExtensionSequenceContract contract = null;
            var extensions = this.Repo.GetExtensionSequenceExtensionsByExtById(extensionId).ToList();
            if (extensions.Count > 0)
            {
                contract = ExtensionSequenceContract.Create(extensions[0].TimeLimitTypeId.GetValueOrDefault(),
                    extensions[0].ExtensionSequence.GetValueOrDefault(), extensions);
            }

            return contract;
        }

        public UpsertResponse<ExtensionSequenceContract> UpsertData(ExtensionContract contract)
        {
            var response = new UpsertResponse<ExtensionSequenceContract>();

            try
            {
                if (this.Participant == null)
                {
                    throw new InvalidOperationException("Participant not valid.");
                }

                ITimeLimitExtension extension = null;
                if (contract.Id > 0)
                {
                    extension = this.Repo.GetExtensionsById(contract.Id);
                }
                else
                {
                    extension = this.Repo.NewTimeLimitExtension();

                    // TODO: Check for extension that overlaps this one, and it's TYPE
                    // UNLESS WE CAN ADD A DB CONSTRANT
                    // http://stackoverflow.com/questions/12035747/date-range-overlapping-check-constraint

                    if (contract.ExtensionDecisionId == (Int32)ExtensionDecision.Approve)
                    {
                        ITimeLimitExtension overlappingExt = this.Repo.GetExensionByDateRange(this.Participant.Id, contract.TimelimitTypeId, contract.StartDate.GetValueOrDefault().DateTime, contract.EndDate.GetValueOrDefault().DateTime);
                        if (overlappingExt != null)
                        {
                            // instead of creating a new extension sequence, we will append to the one that has the overlapping extension (that isn't current) so that deleting
                            // the current decision doesn't cause overlapping decisions
                            contract.SequenceId = overlappingExt.ExtensionSequence.GetValueOrDefault();
                        }
                    }
                }

                this.MapExtensionContractToTimelimitExtensionModel(contract, extension);

                if (contract.SequenceId == default(Int32))
                {
                    // If we are trying to create a "new" sequence, find the highest sequence number for the participnts clock and increment by 1
                    var sequenceNumber = this.Repo.GetCurrentExtensionSequenceNumber(this.Participant.Id, contract.TimelimitTypeId);
                    extension.ExtensionSequence = sequenceNumber.GetValueOrDefault() + 1;
                }

                // TODO: Add concurrency validation for overlapping extensions

                extension.DecisionDate = contract.DecisionDate.GetValueOrDefault(DateTime.Now).DateTime;

                ITimeLimitSummary tls = null;
                if (Repo.GetExtensionDecision(extension.ExtensionDecisionId) == "Approve")
                {
                    var tlType = Repo.GetTimeLimitType(extension.TimeLimitTypeId);
                    var beginMonth = extension.BeginMonth ?? DateTime.Now;
                    var endMonth = extension.EndMonth ?? DateTime.Now;
                    var extensionMonths = ((endMonth.Year - beginMonth.Year) * 12) + endMonth.Month - beginMonth.Month + 1;

                    var timeline = _timelimitService.GetTimeline(Participant.PinNumber.GetValueOrDefault());
                    tls      = _timelimitService.CreateTimelimitSummary(timeline, Participant.Id);
                    tls.ModifiedBy = this.AuthUser?.Username ?? "unknown";

                    switch (tlType)
                    {
                        case "STATE":
                            tls.StateMax += extensionMonths;
                            break;
                        case "CSJ":
                            tls.CSJMax += extensionMonths;
                            break;
                        case "W-2 T":
                            tls.W2TMax += extensionMonths;
                            break;
                        case "TNP":
                        case "TMP":
                            tls.TempMax += extensionMonths;
                            break;
                    }
                }

                WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = true;
                var executionStrategy = new DcfDbExecutionStrategy(5, TimeSpan.FromSeconds(30));
                executionStrategy.Execute(() =>
                {

                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    try
                    {
                        this.Repo.Save();
                        if (Repo.GetExtensionDecision(extension.ExtensionDecisionId) == "Approve")
                            _timelimitService.SaveEntity(tls);
                        this._db2TimelimitService.Upsert(extension, this.Participant, this.AuthUser.Username);
                        response.UpdatedModel = this.GetExtensionSequenceById(extension.Id);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessage = ex.Message;
                        this.Logger.FatalException("Error performing UpsertData in ExtensionSequenceViewModel for @Participant, @data", ex, Participant.PinNumber, contract); ;
                        throw;
                    }
                }
                });
                WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = false;

            }
            catch (DBConcurrencyException ex)
            {
                response.HasConcurrencyError = true;
                response.ErrorMessage = ex.Message;
                throw;
            }

            return response;
        }

        private void MapExtensionContractToTimelimitExtensionModel(ExtensionContract contract, ITimeLimitExtension extension)
        {
            this.MapBaseContractToBaseModel(contract, extension);
            extension.ParticipantId = this.Participant.Id;
            extension.ExtensionSequence = contract.SequenceId;
            extension.TimeLimitTypeId = contract.TimelimitTypeId;
            extension.ApprovalReasonId = contract.ApprovalReasonId;
            extension.DenialReasonId = contract.DenialReasonId;
            extension.BeginMonth = contract.StartDate?.DateTime;
            extension.EndMonth = contract.EndDate?.DateTime;
            extension.DecisionDate = contract.DecisionDate.GetValueOrDefault(DateTime.Now).DateTime;
            extension.Details = contract.ReasonDetails;
            extension.InitialDiscussionDate = contract.InitialDiscussionDate;
            extension.ExtensionDecisionId = contract.ExtensionDecisionId;
            extension.IsPendingDVR = contract.DvrReferralPending;
            extension.IsPendingSSIorSSDI = contract.PendingSSAppOrAppeal;
            extension.IsReceivingDVR = contract.RecieivingDvrServices;
            extension.Notes = contract.Notes;
            extension.IsBackDatedExtenstion = contract.IsBackdated ? 1 : 0;
        }

        public IT0754_LTR_RQST GenerateExtensionNotice(Int32 id)
        {
            if (this.Participant == null)
            {
                throw new InvalidOperationException("Participant not valid.");
            }

            var extension = this.Repo.GetExtensionsById(id);

            var timeline = this._timelimitService.GetTimeline(Participant.PinNumber.GetValueOrDefault());


            return this._db2TimelimitService.CreateExtensionNotice(extension, this.Participant, timeline);
        }

        public IT0754_LTR_RQST InsertExtensionNotice(Int32 id)
        {
            if (this.Participant == null)
            {
                throw new InvalidOperationException("Participant not valid.");
            }

            var extension = this.Repo.GetExtensionsById(id);

            var timeline = this._timelimitService.GetTimeline(Participant.PinNumber.GetValueOrDefault());

            return this._db2TimelimitService.InsertExtensionNotice(extension, this.Participant, timeline);
        }
    }
}