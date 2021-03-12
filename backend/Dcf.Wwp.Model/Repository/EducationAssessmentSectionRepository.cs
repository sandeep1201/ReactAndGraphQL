using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEducationAssessmentSectionRepository
    {
        public IEducationAssessmentSection NewEducationAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new EducationAssessmentSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user
            };
            parentAssessment.EducationAssessmentSection = section;
            _db.EducationAssessmentSections.Add(section);

            return section;
        }
    }
}