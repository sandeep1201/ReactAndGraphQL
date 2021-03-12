using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ILegalIssuesAssessmentSectionRepository 
    {
        public ILegalIssuesAssessmentSection NewLegalIssuesAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new LegalIssuesAssessmentSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user
            };
            parentAssessment.LegalIssuesAssessmentSection = section;
            _db.LegalIssuesAssessmentSections.Add(section);

            return section;
        }
    }
}