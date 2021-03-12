using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWorkProgramAssessmentSectionRepository
    {
        public IWorkProgramAssessmentSection NewWorkProgramAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new WorkProgramAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.WorkProgramAssessmentSection = section;
            _db.WorkProgramAssessmentSections.Add(section);

            return section;
        }
    }
}