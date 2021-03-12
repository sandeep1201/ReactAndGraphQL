using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        private List<CommentContract> GetComments(IEnumerable<EAComment> comments)
        {
            var contracts = new List<CommentContract>();

            comments.ForEach(i => contracts.Add(GetComment(i)));

            return contracts;
        }

        private CommentContract GetComment(EAComment comment)
        {
            if (comment == null) return null;
            var contract = new CommentContract
                           {
                               Id          = comment.Id,
                               CommentText = comment.Comment,
                               CommentTypes = comment.EaCommentTypeBridges
                                                     ?.Where(i => !i.IsDeleted)
                                                     .Select(j => new CommentTypeContract
                                                                  {
                                                                      CommentTypeId   = j.CommentTypeId,
                                                                      CommentTypeName = j.EaCommentType?.Name
                                                                  }).ToList(),
                               IsEdited     = comment.IsEdited,
                               CreatedDate  = comment.CreatedDate,
                               ModifiedBy   = _convertWIUIdToName(comment.ModifiedBy),
                               ModifiedDate = comment.ModifiedDate,
                               WIUID        = comment.ModifiedBy
                           };

            contract.CommentTypeIds = contract.CommentTypes?.Select(x => x.CommentTypeId).ToList();

            return contract;
        }

        public CommentContract UpsertComments(CommentContract eaCommentContract, int requestId)
        {
            if (eaCommentContract == null)
            {
                throw new ArgumentNullException(nameof(eaCommentContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var comment      = eaCommentContract.Id != 0 ? _eaCommentRepository.Get(i => i.Id == eaCommentContract.Id) : _eaCommentRepository.New();

            comment.RequestId    = requestId;
            comment.Comment      = eaCommentContract.CommentText;
            comment.IsEdited     = eaCommentContract.Id != 0;
            comment.IsDeleted    = false;
            comment.ModifiedBy   = modifiedBy;
            comment.ModifiedDate = modifiedDate;

            if (eaCommentContract.Id != 0)
            {
                var commentTypes = comment.EaCommentTypeBridges?.Where(i => !i.IsDeleted).ToList();

                commentTypes?.ForEach(i => i.IsDeleted = true);
                UpsertCommentTypes(eaCommentContract, commentTypes, comment, modifiedBy, modifiedDate);

                _eaCommentRepository.Update(comment);
            }
            else
            {
                comment.CreatedDate = modifiedDate;
                UpsertCommentTypes(eaCommentContract, null, comment, modifiedBy, modifiedDate);

                _eaCommentRepository.Add(comment);
            }

            _unitOfWork.Commit();

            return GetComment(comment);
        }

        private void UpsertCommentTypes(CommentContract eaCommentContract, IReadOnlyCollection<EACommentTypeBridge> commentTypes, EAComment comment, string modifiedBy, DateTime modifiedDate)
        {
            eaCommentContract.CommentTypes
                             .Select(i => i.CommentTypeId)
                             .ForEach(i =>
                                      {
                                          var commentType = commentTypes?.FirstOrDefault(j => j.CommentTypeId == i) ?? _eaCommentTypeBridgeRepository.New();

                                          commentType.EaComment     = comment;
                                          commentType.CommentTypeId = i;
                                          commentType.IsDeleted     = false;
                                          commentType.ModifiedBy    = modifiedBy;
                                          commentType.ModifiedDate  = modifiedDate;

                                          if (commentType.Id == 0)
                                              comment.EaCommentTypeBridges.Add(commentType);
                                      });
        }

        #endregion
    }
}
