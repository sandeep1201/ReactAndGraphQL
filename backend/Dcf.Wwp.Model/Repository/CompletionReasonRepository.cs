using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ICompletionReasonRepository
    {
        public IEnumerable<ICompletionReason> GetCompletionReasonsforEnrolledProgram(string p)
        {
            var q = _db.CompletionReasons.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(p))
            {
                p = p.Trim();
                q = q.Where(i => i.EnrolledProgram.ProgramCode == p && !i.IsSystemUseOnly).OrderBy(i => i.SortOrder);
            }
            else
            {
                q = q.OrderBy(i => i.EnrolledProgramId).ThenBy(i => i.SortOrder);
            }

            q = q.Select(i => i);

            var r = q.ToList();

            return (r);
        }

        public IEnumerable<ICompletionReason> GetCompletionReasonsforEnrolledProgram(Expression<Func<ICompletionReason, bool>> clause)
        {
            var results = new List<ICompletionReason>();

            results = _db.CompletionReasons
                         .Where(clause)
                         .OrderBy(i => i.SortOrder)
                         .Select(i => i)
                         .AsNoTracking()
                         .ToList();

            return (results);
        }
    }
}
