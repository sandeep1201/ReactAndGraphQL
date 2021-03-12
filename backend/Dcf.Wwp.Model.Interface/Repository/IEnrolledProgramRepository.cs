using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IEnrolledProgramRepository
    {
        IEnumerable<IEnrolledProgram> EnrolledPrograms();
        IEnumerable<IEnrolledProgram> NonEligibiltyEnrolledPrograms();
        IEnumerable<IEnrolledProgram> WhereEnrolledPrograms(Expression<Func<IEnrolledProgram, bool>> clause);
    }
}
