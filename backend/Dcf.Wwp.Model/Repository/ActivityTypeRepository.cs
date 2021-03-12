using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IActivityTypeRepository
    {
        public IEnumerable<IEnrolledProgramEPActivityTypeBridge> ActivityTypes(string progCode)
        {
            var q = _db.EnrolledProgramEPActivityTypeBridges.AsQueryable().AsNoTracking();
            var r = new List<EnrolledProgramEPActivityTypeBridge>();

            switch (progCode.ToLower())
            {
                case "w-2":
                case "w2":
                case "ww":
                    r = q.Where(i => i.EnrolledProgramId == Interface.Constants.EnrolledProgram.WW && !i.IsDeleted)
                         .OrderBy(i => i.EnrolledProgram.SortOrder)
                         .Select(i => i)
                         .ToList();
                    break;

                case "lf":
                    r = q.Where(i => i.EnrolledProgramId == Interface.Constants.EnrolledProgram.LearnFareId && !i.IsDeleted)
                         .OrderBy(i => i.EnrolledProgram.SortOrder)
                         .Select(i => i)
                         .ToList();
                    break;

                case "cf":
                    r = q.Where(i => i.EnrolledProgramId == Interface.Constants.EnrolledProgram.ChildrenFirstId && !i.IsDeleted)
                         .OrderBy(i => i.EnrolledProgram.SortOrder)
                         .Select(i => i)
                         .ToList();
                    break;

                case "tjtmj":
                    r = q.Where(i => i.EnrolledProgramId == Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId && !i.IsDeleted)
                         .OrderBy(i => i.EnrolledProgram.SortOrder)
                         .Select(i => i)
                         .ToList();
                    break;
                
                case "tj":
                    r = q.Where(i => i.EnrolledProgramId == Interface.Constants.EnrolledProgram.TransitionalJobsId && !i.IsDeleted)
                         .OrderBy(i => i.EnrolledProgram.SortOrder)
                         .Select(i => i)
                         .ToList();
                    break;

                case "tmj":
                    r = q.Where(i => i.EnrolledProgramId == Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId && !i.IsDeleted)
                         .OrderBy(i => i.EnrolledProgram.SortOrder)
                         .Select(i => i)
                         .ToList();
                    break;

                default:
                    throw new NotImplementedException(progCode);
            }
            
            return (r);
        }
    }
}
