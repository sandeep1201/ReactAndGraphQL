using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class PinCommentDomain : IPinCommentDomain
    {
        #region Properties

        private readonly IPinCommentRepository  _pinCommentRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IPCCTBridgeRepository  _pcctBridgeRepository;
        private readonly IUnitOfWork            _unitOfWork;
        private readonly IAuthUser              _authUser;
        private readonly Func<string, string>   _convertWIUIdToName;

        #endregion

        #region Methods

        public PinCommentDomain(IPinCommentRepository  pinCommentRepository,
                                IParticipantRepository participantRepository,
                                IPCCTBridgeRepository  pcctBridgeRepository,
                                IUnitOfWork            unitOfWork,
                                IAuthUser              authUser,
                                IWorkerRepository      workerRepo)
        {
            _pinCommentRepository  = pinCommentRepository;
            _participantRepository = participantRepository;
            _pcctBridgeRepository  = pcctBridgeRepository;
            _unitOfWork            = unitOfWork;
            _authUser              = authUser;

            _convertWIUIdToName = (wiuId) =>
                                  {
                                      var wo = workerRepo.GetAsQueryable()
                                                         .Where(i => i.WIUId == wiuId)
                                                         .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                         .FirstOrDefault();

                                      var wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");

                                      return (wn);
                                  };
        }


        public List<CommentContract> GetPinComments(decimal pin)
        {
            var contracts     = new List<CommentContract>();
            var participantId = _participantRepository.Get(i => i.PinNumber == pin).Id;
            var comments      = _pinCommentRepository.GetMany(i => i.ParticipantId == participantId && i.IsDeleted == false).OrderByDescending(i => i.ModifiedDate).ToList();

            comments.ForEach(i => contracts.Add(
                                                new CommentContract
                                                {
                                                    Id          = i.Id,
                                                    CommentText = i.CommentText,
                                                    CommentTypes = i.PCCTBridges.Select(j => new CommentTypeContract
                                                                                             {
                                                                                                 CommentTypeId   = j.CommentTypeId,
                                                                                                 CommentTypeName = j.PinCommentType.Name,
                                                                                                 IsSystemUseOnly = j.PinCommentType.SystemUseOnly
                                                                                             }).ToList(),
                                                    IsEdited     = i.IsEdited,
                                                    CreatedDate  = i.CreatedDate,
                                                    ModifiedBy   = _convertWIUIdToName(i.ModifiedBy),
                                                    ModifiedDate = i.ModifiedDate,
                                                    WIUID        = i.ModifiedBy
                                                }));
            return contracts;
        }

        public CommentContract GetPinComment(int id)
        {
            CommentContract contract = null;
            var             comment  = _pinCommentRepository.Get(i => i.Id == id);

            if (comment != null)
            {
                contract = new CommentContract
                           {
                               Id          = comment.Id,
                               CommentText = comment.CommentText,
                               CommentTypes = comment.PCCTBridges?.Select(j => new CommentTypeContract
                                                                               {
                                                                                   CommentTypeId   = j.CommentTypeId,
                                                                                   CommentTypeName = j.PinCommentType?.Name
                                                                               }).ToList(),
                               IsEdited     = comment.IsEdited,
                               CreatedDate  = comment.CreatedDate,
                               ModifiedBy   = _convertWIUIdToName(comment.ModifiedBy),
                               ModifiedDate = comment.ModifiedDate,
                               WIUID        = comment.ModifiedBy
                           };

                contract.CommentTypeIds = contract.CommentTypes?.Select(x => x.CommentTypeId).ToList();
            }

            return contract;
        }

        public CommentContract UpsertPinComment(CommentContract pinCommentContract, string pin, int id)
        {
            CommentContract contract = null;

            if (pinCommentContract == null)
            {
                throw new ArgumentNullException(nameof(pinCommentContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var decimalPin   = decimal.Parse(pin);
            var participant  = _participantRepository.Get(i => i.PinNumber == decimalPin);

            if (pinCommentContract.Id != 0)
            {
                var comment = _pinCommentRepository.Get(i => i.Id == id && i.IsDeleted == false);

                if (comment != null)
                {
                    comment.Participant  = participant;
                    comment.CommentText  = pinCommentContract.CommentText;
                    comment.IsEdited     = true;
                    comment.IsDeleted    = false;
                    comment.ModifiedBy   = modifiedBy;
                    comment.ModifiedDate = modifiedDate;

                    if (comment.PCCTBridges != null)
                    {
                        var allIds        = comment.PCCTBridges?.Select(i => i.CommentTypeId).ToList();
                        var contractIds   = pinCommentContract.CommentTypes?.Select(i => i.CommentTypeId).ToList();
                        var idsToDelete   = allIds.Except(contractIds.AsNotNull()).ToList();
                        var idsToAdd      = contractIds.AsNotNull().Except(allIds.AsNotNull()).ToList();
                        var typesToDelete = comment.PCCTBridges.Where(i => idsToDelete.Contains(i.CommentTypeId)).Select(i => i).ToList();
                        var typesToUpdate = comment.PCCTBridges.Where(i => contractIds.AsNotNull().Contains(i.CommentTypeId)).Select(i => i).ToList();
                        var typesToAdd    = pinCommentContract.CommentTypes?.Where(i => idsToAdd.Contains(i.CommentTypeId)).Select(i => i).ToList();

                        typesToDelete.ForEach(type => _pcctBridgeRepository.Delete(type));

                        foreach (var type in typesToUpdate.AsNotNull().Select(typeFromContract => comment.PCCTBridges.FirstOrDefault(i => i.CommentTypeId == typeFromContract.CommentTypeId)).Where(type => type != null))
                        {
                            type.ModifiedBy   = modifiedBy;
                            type.ModifiedDate = modifiedDate;
                        }

                        foreach (var type in typesToAdd.AsNotNull().Select(typeFromContract => new PCCTBridge
                                                                                               {
                                                                                                   PinComment    = comment,
                                                                                                   CommentTypeId = typeFromContract.CommentTypeId,
                                                                                                   IsDeleted     = false,
                                                                                                   ModifiedBy    = modifiedBy,
                                                                                                   ModifiedDate  = modifiedDate
                                                                                               }))
                        {
                            comment.PCCTBridges.Add(type);
                        }
                    }

                    _pinCommentRepository.Update(comment);
                    _unitOfWork.Commit();
                    contract = GetPinComment(comment.Id);
                }
            }
            else
            {
                var comment = _pinCommentRepository.New();

                comment.Participant  = participant;
                comment.CommentText  = pinCommentContract.CommentText;
                comment.IsEdited     = false;
                comment.IsDeleted    = false;
                comment.CreatedDate  = modifiedDate;
                comment.ModifiedBy   = modifiedBy;
                comment.ModifiedDate = modifiedDate;

                foreach (var type in pinCommentContract.CommentTypes.AsNotNull().Select(typeFromContract => new PCCTBridge
                                                                                                            {
                                                                                                                PinComment    = comment,
                                                                                                                CommentTypeId = typeFromContract.CommentTypeId,
                                                                                                                IsDeleted     = false,
                                                                                                                ModifiedBy    = modifiedBy,
                                                                                                                ModifiedDate  = modifiedDate
                                                                                                            }))
                {
                    comment.PCCTBridges.Add(type);
                }

                _pinCommentRepository.Add(comment);
                _unitOfWork.Commit();
                contract = GetPinComment(comment.Id);
            }


            return contract;
        }

        #endregion
    }
}
