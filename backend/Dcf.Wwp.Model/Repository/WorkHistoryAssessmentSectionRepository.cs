using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWorkHistoryAssessmentSectionRepository
    {
        public IWorkHistoryAssessmentSection NewWorkHistoryAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new WorkHistoryAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.WorkHistoryAssessmentSection = section;
            _db.WorkHistoryAssessmentSections.Add(section);

            return section;
        }
    }
}