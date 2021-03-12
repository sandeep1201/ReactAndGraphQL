
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRequestForAssistanceStatusRepository
    {
        IRequestForAssistanceStatus              GetRequestForAssistanceStatus(string statusName);
        IEnumerable<IRequestForAssistanceStatus> GetRequestForAssistanceStatusesWhere(Expression<Func<IRequestForAssistanceStatus, bool>> clause);
    }
}
