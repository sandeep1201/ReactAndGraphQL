using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DCF.Common.Logging;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        private EAGroupMembersContract GetEaGroupMembers(EARequest request, ICollection<EARequestParticipantBridge> requestParticipants)
        {
            requestParticipants = requestParticipants.Where(i => !i.IsDeleted).ToList();

            var contract = new EAGroupMembersContract { RequestId = request.Id, IsPreviousMemberClicked = request.IsPreviousMemberClicked };

            if (requestParticipants.Count == 0) return contract;

            var parms = new Dictionary<string, object>
                        {
                            ["PinNumbers"]   = string.Join(", ", requestParticipants.Select(i => i.Participant.PinNumber.ToString()).Distinct()),
                            ["ReturnResult"] = true
                        };

            var t0011 = _eaRequestRepository.ExecStoredProc<T0011>("USP_GetSSNFromT0011", parms).ToList();

            contract.EaGroupMembers = requestParticipants.Select(earp =>
                                                                 {
                                                                     var earParticipant = earp.Participant;

                                                                     return new EARequestParticipantContract
                                                                            {
                                                                                Id                   = earp.Id,
                                                                                PinNumber            = earParticipant?.PinNumber,
                                                                                ParticipantId        = earp.ParticipantId,
                                                                                FirstName            = earp.Participant.FirstName,
                                                                                MiddleInitial        = earp.Participant.MiddleInitial,
                                                                                LastName             = earp.Participant.LastName,
                                                                                SuffixName           = earp.Participant.SuffixName,
                                                                                ParticipantDOB       = earp.Participant?.DateOfBirth,
                                                                                EARequestId          = earp.EARequestId,
                                                                                EAIndividualTypeId   = earp.EAIndividualTypeId,
                                                                                EAIndividualTypeCode = earp.EaIndividualType?.Code,
                                                                                EAIndividualTypeName = earp.EaIndividualType?.Name,
                                                                                EARelationTypeId     = earp.EARelationTypeId,
                                                                                EARelationTypeName   = earp.EaRelationshipType?.Name,
                                                                                IsIncluded           = earp.IsIncluded,
                                                                                SSN                  = t0011.FirstOrDefault(i => i.PinNumber == earParticipant?.PinNumber)?.SSN,
                                                                                SSNAppliedDate       = earp.SSNAppliedDate,
                                                                                SSNExemptTypeId      = earp.SSNExemptTypeId,
                                                                                SSNExemptTypeName    = earp.EaSsnExemptType?.Name,
                                                                                ModifiedBy           = _convertWIUIdToName(earp.ModifiedBy),
                                                                                ModifiedDate         = earp.ModifiedDate
                                                                            };
                                                                 }).ToList();

            var recentAGMember = requestParticipants.OrderByDescending(i => i.ModifiedDate).First();

            contract.ModifiedBy               = _convertWIUIdToName(recentAGMember.ModifiedBy);
            contract.ModifiedDate             = recentAGMember.ModifiedDate;
            contract.IsSubmittedViaDriverFlow = requestParticipants.Any(i => i.EaRelationshipType?.Code != _selfCode);

            return contract;
        }

        public List<EAAGMembers> GetAGMembers(string pin, int id)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]  = pin,
                            ["CaseNumber"] = DBNull.Value,
                            ["RequestId"]  = id,
                            ["IsForEA"]    = true
                        };

            var agMembers = _eaRequestRepository.ExecStoredProc<EAAGMembers>("USP_GetDB2AGMembers", parms).ToList();

            return agMembers;
        }

        public T0011 SearchParticipant(string pin)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumbers"]   = pin,
                            ["ReturnResult"] = true
                        };

            var t0011 = _eaRequestRepository.ExecStoredProc<T0011>("USP_GetSSNFromT0011", parms).FirstOrDefault();

            return t0011;
        }

        public EAGroupMembersContract UpsertGroupMembers(EAGroupMembersContract contract)
        {
            var modifiedBy          = _authUser.WIUID;
            var modifiedDate        = DateTime.Now;
            var request             = _eaRequestRepository.Get(i => i.Id == contract.RequestId && !i.IsDeleted);
            var requestParticipants = request.EaRequestParticipantBridges;

            request.IsPreviousMemberClicked = contract.IsPreviousMemberClicked;

            var dbMembers       = requestParticipants.Select(i => i.Id).ToList();
            var contractMembers = contract.EaGroupMembers?.Select(i => i.Id).ToList();
            var missingMembers  = contractMembers == null ? dbMembers : dbMembers.Except(contractMembers).ToList();

            if (missingMembers.Count != 0)
                _logger.Warn($"UpsertGroupMembers warning for missing members in contract: {string.Join(", ", missingMembers)}");

            contract.EaGroupMembers?.ForEach(i =>
                                            {
                                                var bridge        = i.Id == 0 ? _eaRequestParticipantBridgeRepository.New() : requestParticipants.First(j => j.Id == i.Id);
                                                var participantId = i.ParticipantId ?? _participantRepository.Get(j => j.PinNumber                                == i.PinNumber)?.Id;

                                                if (participantId == null)
                                                {
                                                    var parms = new Dictionary<string, object>
                                                                {
                                                                    ["PinNumber"] = i.PinNumber?.ToString(CultureInfo.InvariantCulture) ?? (object) DBNull.Value,
                                                                    ["Debug"]     = true
                                                                };

                                                    var participant = _eaRequestRepository.ExecStoredProc<Participant>("USP_RefreshParticipant", parms).FirstOrDefault();

                                                    participantId = participant?.Id;
                                                }

                                                bridge.ParticipantId      = participantId.GetValueOrDefault();
                                                bridge.EaRequest          = request;
                                                bridge.EAIndividualTypeId = i.EAIndividualTypeId;
                                                bridge.EARelationTypeId   = i.EARelationTypeId;
                                                bridge.IsIncluded         = i.IsIncluded;
                                                bridge.SSNAppliedDate     = i.SSNAppliedDate;
                                                bridge.SSNExemptTypeId    = i.SSNExemptTypeId;
                                                bridge.IsDeleted          = false;
                                                bridge.ModifiedBy         = modifiedBy;
                                                bridge.ModifiedDate       = modifiedDate;

                                                if (i.Id == 0)
                                                    requestParticipants.Add(bridge);
                                            });

            request.ModifiedBy   = modifiedBy;
            request.ModifiedDate = modifiedDate;

            _unitOfWork.Commit();

            return GetEaGroupMembers(request, requestParticipants);
        }

        #endregion
    }
}
