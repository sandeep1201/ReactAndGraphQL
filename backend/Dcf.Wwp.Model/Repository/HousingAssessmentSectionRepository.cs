using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IHousingAssessmentSectionRepository
    {
        public IHousingAssessmentSection NewHousingAssessmentSection(IInformalAssessment parentAssessment, string user)
        {
            var section = new HousingAssessmentSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user
            };
            parentAssessment.HousingAssessmentSection = section;
            _db.HousingAssessmentSections.Add(section);

            return section;
        }
    }
}