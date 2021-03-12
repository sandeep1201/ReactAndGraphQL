using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IMilitaryTrainingAssessmentSectionRepository
    {
        public IMilitaryTrainingAssessmentSection NewMilitaryTrainingAssessmentSection(
            IInformalAssessment parentAssessment, string user)
        {
            var section = new MilitaryTrainingAssessmentSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user
            };
            parentAssessment.MilitaryTrainingAssessmentSection = section;
            _db.MilitaryTrainingAssessmentSections.Add(section);

            return section;
        }
    }
}
