using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEnrolledProgramRepository
    {
        public IEnumerable<IEnrolledProgram> EnrolledPrograms()
        {
            var enrolledProgs = _db.EnrolledPrograms.AsNoTracking();

            return enrolledProgs;
        }

        public IEnumerable<IEnrolledProgram> NonEligibiltyEnrolledPrograms()
        {
            var nonEnrolledProgs = _db.EnrolledPrograms
                                      .Where(i => i.ProgramType == "NonEligibility")
                                      .OrderBy(i => i.SortOrder)
                                      .Select(i => i)
                                      .AsNoTracking();

            return nonEnrolledProgs;
        }

        public IEnumerable<IEnrolledProgram> WhereEnrolledPrograms(Expression<Func<IEnrolledProgram, bool>> clause)
        {
            try
            {
                return _db.EnrolledPrograms
                          .Where(clause)
                          .AsNoTracking();
            }
            catch (NullReferenceException)
            {
                return new List<IEnrolledProgram>();
            }
        }
    }
}
