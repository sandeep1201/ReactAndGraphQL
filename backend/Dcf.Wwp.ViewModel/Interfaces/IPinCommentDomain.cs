using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IPinCommentDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<CommentContract> GetPinComments(decimal              pin);
        CommentContract       GetPinComment(int                   id);
        CommentContract       UpsertPinComment(CommentContract pinCommentContract, string pin, int id);

        #endregion
    }
}
