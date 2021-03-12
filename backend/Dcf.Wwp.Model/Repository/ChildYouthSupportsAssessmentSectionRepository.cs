using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IChildYouthSupportsAssessmentSectionRepository
    {
        public IChildYouthSupportsAssessmentSection NewChildYouthSupportsAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new ChildYouthSupportsAssessmentSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            parentAssessment.ChildYouthSupportsAssessmentSection = section;
            _db.ChildYouthSupportsAssessmentSections.Add(section);

            return section;
        }
    }
}