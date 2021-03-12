using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IFamilyBarriersAssessmentSectionRepository
    {
        public IFamilyBarriersAssessmentSection NewFamilyBarriersAssessmentSection(IInformalAssessment parentAssessment,
            string user)
        {
            var section = new FamilyBarriersAssessmentSection
            {
                ModifiedDate = DateTime.Now,
                ModifiedBy = user
            };
            parentAssessment.FamilyBarriersAssessmentSection = section;
            _db.FamilyBarriersAssessmentSections.Add(section);

            return section;
        }
    }
}
