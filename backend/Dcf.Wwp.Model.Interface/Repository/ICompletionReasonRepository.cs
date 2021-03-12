using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ICompletionReasonRepository
    {
        IEnumerable<ICompletionReason> GetCompletionReasonsforEnrolledProgram(string programCd);
        IEnumerable<ICompletionReason> GetCompletionReasonsforEnrolledProgram(Expression<Func<ICompletionReason, bool>> clause);
    }
}
